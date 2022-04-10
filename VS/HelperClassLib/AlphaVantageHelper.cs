using DbConnectionClassLib.Data;
using DbConnectionClassLib.ResponseClasses;
using DbConnectionClassLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HelperClassLib.Helpers.HelperClass;
using static DbConnectionClassLib.Data.DatabaseInstance;
using HelperClassLib.AlphaVantage;

//"Note": "Thank you for using Alpha Vantage! Our standard API call frequency is 5 calls per minute and 500 calls per day. Please visit https://www.alphavantage.co/premium/ if you would like to target a higher API call frequency. Thank you!"

namespace HelperClassLib
{
    public static class AlphaVantageHelper
    {
        public static List<TIME_SERIES_DAILY_ADJUSTED> GetEquityBetweenDates(string instrument, string instrumentFullName, DateTime dtFrom, DateTime dtTo)
        {
            try
            {
                AV_TIME_SERIES_DAILY_ADJUSTED Equity = Db.av_time_series_daily_adjusted.Where(e => e.symbol == instrument).FirstOrDefault();
                int AVTSDA_MAXMINUTESOLD = Db.db_settings.Where(d => d.Key == "AVTSDA_MAXMINUTESOLD").Select(d => int.Parse(d.Value)).FirstOrDefault();
                AV_TIME_SERIES_DAILY_ADJUSTED_DATA last = null;
                string outputsize = "none";
                if (Equity == null)
                {
                    outputsize = "full";
                }
                else
                {
                    last = Db.av_time_series_daily_adjusted_data.Where(d => d.avtsdaId == Equity.id).OrderByDescending(d => d.timestamp).FirstOrDefault();
                    DateTime lastDay = Db.av_time_series_daily_adjusted_data.Where(d => d.avtsdaId == Equity.id).Select(d => d.timestamp).OrderBy(d => d).LastOrDefault();
                    if (lastDay != null && lastDay > DateTime.Today.AddDays(-30))
                        outputsize = "compact";
                    else
                        outputsize = "full";
                    if (Equity.lastupdate.HasValue)
                    {
                        DateTime lastUpdated = Equity.lastupdate.Value.ToUniversalTime();
                        DateTime nowUtc = DateTime.Now.ToUniversalTime();
                        if ((nowUtc - lastUpdated).TotalMinutes < AVTSDA_MAXMINUTESOLD) // Ha nem túl régen lett frissítve, akkor nem kell API hívás
                            outputsize = "none";
                    }
                }
                DateTime lastDate = DateTime.MinValue;
                var newDatas = new List<TIME_SERIES_DAILY_ADJUSTED>();
                bool error = false;
                // TODO: Ha kell frissiteni // TODO EZ VALAMIÉRT NEM JÓ, MÉGIS BEFRISSÍTI
                if (outputsize != "none")
                {
                    if (Equity != null)
                    {
                        if (last != null)
                        {
                            lastDate = last.timestamp.Date;
                            Db.av_time_series_daily_adjusted_data.Remove(last);
                            lastDate = lastDate.AddDays(-1);
                            Db.SaveChanges();
                        }
                    }
                    // TODO: Ha db_log táblában a value: "av_response.Length: 236", akkor azt nem sikerült frissíteni
                    AlphaVantageClient avc = new AlphaVantageClient(new AlphaVantageTimeSeriesParameters(instrument, AlphaVantageDatatype.CSV, AlphaVantageTimeSeriesFunction.DAILY, (AlphaVantageTimeSeriesOutputSize)Enum.Parse(typeof(AlphaVantageTimeSeriesOutputSize), outputsize)));
                    try
                    {
                        var tsda = avc.GetDataTimeSeries();
                        newDatas = tsda.Select(t => new TIME_SERIES_DAILY_ADJUSTED {
                            close = t.close,
                            high = t.high,
                            low = t.low,
                            open = t.open,
                            timestamp = t.timestamp,
                            volume = t.volume
                        }).ToList();
                    }
                    catch (Exception avEx)
                    {
                        if (avEx.Message == "Alpha Vantage API Call Frequency Exception")
                            error = true;
                    }
                }

                // Van frissítendő adat
                if (newDatas != null && newDatas.Any())
                {
                    // Már létező frissítése új adatokkal
                    if (Equity != null && !error) 
                    {
                        Equity.state = -1;
                        Db.SaveChanges();
                        var av_time_series_daily_adjusted_datas = from d in newDatas
                                                                  where d.timestamp > lastDate
                                                                  select new AV_TIME_SERIES_DAILY_ADJUSTED_DATA
                                                                  {
                                                                      avtsdaId = Equity.id,
                                                                      timestamp = d.timestamp,
                                                                      adjusted_close = d.adjusted_close,
                                                                      close = d.close,
                                                                      high = d.high,
                                                                      low = d.low,
                                                                      open = d.open,
                                                                      volume = d.volume,
                                                                      dividend_amount = d.dividend_amount,
                                                                      split_coefficient = d.split_coefficient
                                                                  };
                        Db.av_time_series_daily_adjusted_data.AddRange(av_time_series_daily_adjusted_datas);
                        var result = Db.SaveChanges();
                        Equity.state = result;
                        Equity.lastupdate = DateTime.Now.ToUniversalTime();
                        Db.SaveChanges();
                    }

                    // Teljesen új, és lett letöltve adat
                    if (Equity == null)
                    {
                        Equity = new AV_TIME_SERIES_DAILY_ADJUSTED { lastupdate = DateTime.MinValue, state = -1, name = instrumentFullName, symbol = instrument };
                        Db.av_time_series_daily_adjusted.Add(Equity);
                        Db.SaveChanges();
                        var av_time_series_daily_adjusted_datas = from d in newDatas
                                                                  select new AV_TIME_SERIES_DAILY_ADJUSTED_DATA
                                                                  {
                                                                      avtsdaId = Equity.id,
                                                                      timestamp = d.timestamp,
                                                                      adjusted_close = d.adjusted_close,
                                                                      close = d.close,
                                                                      high = d.high,
                                                                      low = d.low,
                                                                      open = d.open,
                                                                      volume = d.volume,
                                                                      dividend_amount = d.dividend_amount,
                                                                      split_coefficient = d.split_coefficient
                                                                  };
                        Db.av_time_series_daily_adjusted_data.AddRange(av_time_series_daily_adjusted_datas);
                        var result = Db.SaveChanges();
                        Equity.state = result;
                        Equity.lastupdate = DateTime.Now.ToUniversalTime();
                        Db.SaveChanges();
                    }
                }

                return Db.av_time_series_daily_adjusted_data.Where(d => d.avtsdaId == Equity.id && d.timestamp >= dtFrom && d.timestamp <= dtTo)
                                                            .OrderBy(d => d.timestamp)
                                                            .Select(d => new TIME_SERIES_DAILY_ADJUSTED
                                                            {
                                                                timestamp = d.timestamp,
                                                                volume = d.volume,
                                                                dividend_amount = d.dividend_amount,
                                                                adjusted_close = d.adjusted_close,
                                                                close = d.close,
                                                                high = d.high,
                                                                low = d.low,
                                                                open = d.open
                                                            }).ToList();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid API call")) // Nem AV letezo symbol, vagy mar nem elerheto
                {
                    AV_TIME_SERIES_DAILY_ADJUSTED Equity = Db.av_time_series_daily_adjusted.Where(e => e.symbol == instrument).FirstOrDefault();
                    Equity.state = -9999;
                    Db.SaveChanges();
                    throw ex;
                }
                if (ex.Message.Contains("Thank you for using Alpha Vantage! Our standard API call frequency is"))
                {
                    // TODO: ITT Ha tul sürü lekérdezés jön, akkor előrébb hozhatná a lekérdezési listában, kell neki egy külön flag pl (Equity.state = -10)
                    AV_TIME_SERIES_DAILY_ADJUSTED Equity = Db.av_time_series_daily_adjusted.Where(e => e.symbol == instrument).FirstOrDefault();
                    return Db.av_time_series_daily_adjusted_data.Where(d => d.avtsdaId == Equity.id && d.timestamp >= dtFrom && d.timestamp <= dtTo)
                                            .OrderBy(d => d.timestamp)
                                            .Select(d => new TIME_SERIES_DAILY_ADJUSTED
                                            {
                                                timestamp = d.timestamp,
                                                volume = d.volume,
                                                dividend_amount = d.dividend_amount,
                                                adjusted_close = d.adjusted_close,
                                                close = d.close,
                                                high = d.high,
                                                low = d.low,
                                                open = d.open
                                            }).ToList();
                }
                else
                {
                    throw ex;
                }
            }
        }

        private static void UpdateSymbols(List<AV_SYMBOL_SEARCH_RESPONSE> symbolResults)
        {
            try
            {
                List<string> existingSymbols = Db.av_time_series_daily_adjusted
                                                 .Where(a => symbolResults.Select(sr => sr.symbol).Contains(a.symbol))
                                                 .Select(a => a.symbol).ToList();
                List<AV_SYMBOL_SEARCH_RESPONSE> NewSymbols = symbolResults.Where(s => !existingSymbols.Contains(s.symbol)).ToList();
                List<AV_TIME_SERIES_DAILY_ADJUSTED> NewAlphaVantageSymbols = NewSymbols.Select(n => new AV_TIME_SERIES_DAILY_ADJUSTED
                {
                    name = n.name,
                    symbol = n.symbol,
                    state = 0
                }).ToList();
                Db.av_time_series_daily_adjusted.AddRange(NewAlphaVantageSymbols);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        public async static void UpdateClosePrices()
        {
            Console.WriteLine("     =====    UpdateClosePrices()     =====    " + DateTime.Now.ToDateTimeString());
            try
            {
                int AVTSDA_MAXMINUTESOLD = Db.db_settings.Where(d => d.Key == "AVTSDA_MAXMINUTESOLD").Select(d => int.Parse(d.Value)).FirstOrDefault();
                List<AV_TIME_SERIES_DAILY_ADJUSTED> frisstendoAdatok = (from avtsda in Db.av_time_series_daily_adjusted
                                                                        where avtsda.lastupdate.HasValue &&
                                                                              avtsda.state != -9999 && avtsda.state != -8888
                                                                        orderby avtsda.lastupdate
                                                                        select avtsda).ToList();
                List<string> Favorites = (from fav in Db.Favorites
                                          where fav.Type.StartsWith("Equities/") &&
                                                fav.Type.Contains(".") == false &&
                                                fav.Type.Contains("^") == false
                                          select fav.Type).ToList().Select(f => f.Substring(9, f.Length - 9)).ToList();
                frisstendoAdatok = frisstendoAdatok.Where(f => Favorites.Contains(f.symbol)).OrderBy(f => f.lastupdate).Take(4).ToList();
                

                foreach (AV_TIME_SERIES_DAILY_ADJUSTED frisstendo in frisstendoAdatok)
                {
                    if ((DateTime.Now - frisstendo.lastupdate.Value).TotalMinutes < AVTSDA_MAXMINUTESOLD) continue;

                    AV_TIME_SERIES_DAILY_ADJUSTED_DATA lastRow = (from avtsdad in Db.av_time_series_daily_adjusted_data
                                                                  where avtsdad.avtsdaId == frisstendo.id
                                                                  orderby avtsdad.timestamp descending
                                                                  select avtsdad).FirstOrDefault();
                    DateTime from = new DateTime(1900, 1, 1);
                    if (lastRow != null)
                        from = lastRow.timestamp;
                    try
                    {
                        Console.WriteLine("\n\nUpdateSymbols() " + frisstendo.symbol);
                        List<TIME_SERIES_DAILY_ADJUSTED> result = HelperClassLib.Helpers.HelperClass.Helper.GetInstrumentDataBetweenDates(from, DateTime.Today, DbConnectionClassLib.Parameters.instrumentType.Equities, frisstendo.symbol).Result;
                        Console.WriteLine("\nUpdateSymbols() " + frisstendo.symbol + " OK");
                        GetAndUpdateOverview(frisstendo.symbol);
                    }
                    catch (Exception updateEx)
                    {
                        Console.WriteLine("\nUpdateSymbols() " + frisstendo.symbol + " ERR" + updateEx.Message);
                        frisstendo.state = -8888;
                        Db.SaveChanges();
                        HelperClassLib.Helpers.HelperClass.Helper.LogException(updateEx);
                    }
                    await Task.Delay(20000);
                }
            }
            catch (Exception ex)
            {
                HelperClassLib.Helpers.HelperClass.Helper.LogException(ex);
            }
        }

        public static string GetAndUpdateOverview(string symbol)
        {
            try
            {
                var avtsda = Db.av_time_series_daily_adjusted.Where(a => a.symbol == symbol).FirstOrDefault();
                if (avtsda == null)
                {
                    throw new Exception("(avtsda == null)");
                }
                AlphaVantageClient avc = new AlphaVantageClient(new AlphaVantageOverviewParameters(symbol));
                var av_overview = Db.av_overview.Where(o => o.avtsdaId == avtsda.id).FirstOrDefault();
                if (av_overview == null)
                {
                    string overview = avc.GetDataOverview();
                    if (overview.Contains("Thank you for using Alpha Vantage")) // tul gyakori lekerdezes
                    {
                        throw new Exception("too frequet av api call");
                    }
                    av_overview = new AV_OVERVIEW()
                    {
                        avtsdaId = avtsda.id,
                        data = overview,
                        timestamp = DateTime.Now
                    };
                    Db.av_overview.Add(av_overview);
                    Db.SaveChanges();
                }
                else
                {
                    int AV_OVERVIEW_MAXDAYSOLD = Db.db_settings.Where(d => d.Key == "AV_OVERVIEW_MAXDAYSOLD").Select(d => int.Parse(d.Value)).FirstOrDefault();
                    if ((DateTime.Now - av_overview.timestamp).TotalDays > AV_OVERVIEW_MAXDAYSOLD)
                    {
                        string overview = avc.GetDataOverview();
                        if (overview.Contains("Thank you for using Alpha Vantage") || overview.Contains("Exception")) // tul gyakori lekerdezes
                        {
                            return av_overview.data;
                        }
                        av_overview.data = overview;
                        av_overview.timestamp = DateTime.Now;
                        Db.SaveChanges();
                    }
                }
                return av_overview.data;
            }
            catch (Exception ex)
            {
                return "error: " + ex.Message;
            }
        }
    }
}