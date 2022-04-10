using DbConnectionClassLib.Parameters;
using DbConnectionClassLib.ResponseClasses;
using DbConnectionClassLib.Tables;
using HelperClassLib.AlphaVantage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static DbConnectionClassLib.Data.DatabaseInstance;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        public async Task<List<TIME_SERIES_DAILY_ADJUSTED>> GetInstrumentDataBetweenDates(DateTime dtFrom, DateTime dtTo, instrumentType type, string instrument, string instrumentFullName = null, CandleInterval interval = CandleInterval.Daily)
        {
            List<TIME_SERIES_DAILY_ADJUSTED> datasWithPriorData = new List<TIME_SERIES_DAILY_ADJUSTED>();
            try
            {
                switch (type)
                {
                    case instrumentType.Indexes:
                    case instrumentType.Equities:
                        AlphaVantageClient avc = null;
                        switch (interval)
                        {
                            case CandleInterval.Daily:
                            case CandleInterval.Weekly:
                                datasWithPriorData = AlphaVantageHelper.GetEquityBetweenDates(instrument, instrumentFullName, dtFrom, dtTo);
                                break;
                            case CandleInterval.Min_1:
                                avc = new AlphaVantageClient(new AlphaVantageTimeSeriesParameters(instrument, AlphaVantageDatatype.CSV, AlphaVantageTimeSeriesFunction.INTRADAY, AlphaVantageTimeSeriesOutputSize.full, AlphaVantageTimeSeriesIntradayInterval.min1));
                                break;
                            case CandleInterval.Min_5:
                                avc = new AlphaVantageClient(new AlphaVantageTimeSeriesParameters(instrument, AlphaVantageDatatype.CSV, AlphaVantageTimeSeriesFunction.INTRADAY, AlphaVantageTimeSeriesOutputSize.full, AlphaVantageTimeSeriesIntradayInterval.min5));
                                break;
                            case CandleInterval.Min_15:
                                avc = new AlphaVantageClient(new AlphaVantageTimeSeriesParameters(instrument, AlphaVantageDatatype.CSV, AlphaVantageTimeSeriesFunction.INTRADAY, AlphaVantageTimeSeriesOutputSize.full, AlphaVantageTimeSeriesIntradayInterval.min15));
                                break;
                            case CandleInterval.Min_30:
                                avc = new AlphaVantageClient(new AlphaVantageTimeSeriesParameters(instrument, AlphaVantageDatatype.CSV, AlphaVantageTimeSeriesFunction.INTRADAY, AlphaVantageTimeSeriesOutputSize.full, AlphaVantageTimeSeriesIntradayInterval.min30));
                                break;
                            case CandleInterval.Min_60:
                                avc = new AlphaVantageClient(new AlphaVantageTimeSeriesParameters(instrument, AlphaVantageDatatype.CSV, AlphaVantageTimeSeriesFunction.INTRADAY, AlphaVantageTimeSeriesOutputSize.full, AlphaVantageTimeSeriesIntradayInterval.min60));
                                break;
                        }
                        //datasWithPriorData = avc.GetData();
                        if (interval != CandleInterval.Daily && interval != CandleInterval.Weekly)
                        {
                            //datasWithPriorData = avc.GetDataTimeSeries();
                            var tsintraday = avc.GetDataTimeSeries();
                            datasWithPriorData = tsintraday.Select(t => new TIME_SERIES_DAILY_ADJUSTED
                            {
                                close = t.close,
                                high = t.high,
                                low = t.low,
                                open = t.open,
                                timestamp = t.timestamp,
                                volume = t.volume
                            }).ToList();
                        }
                        break;
                    case instrumentType.HungarianEquitiesIntraday:
                        datasWithPriorData = await PortfolioHelper.GetHungarianEquityIntraday(instrument);
                        if (datasWithPriorData.Any() == false)
                        {
                            datasWithPriorData = await PortfolioHelper.GetHungarianEquityIntraday(instrument, true);
                        }
                        if (datasWithPriorData.Count > 50)
                        {
                            datasWithPriorData = GenerateCandlePoints(datasWithPriorData, interval);
                        }
                        break;

                    case instrumentType.HungarianEquities:
                        portfolio_hunstock equity = Db.portfolio_hunstock.Where(s => s.name == instrument).FirstOrDefault();
                        datasWithPriorData = await PortfolioHelper.GetHungarianEquityBetweenDates(equity.ticker, dtFrom, dtTo);
                        var zerosOpen = datasWithPriorData.Where(d => d.open == 0);


                        datasWithPriorData.Select(d => {
                            if (d.open == 0 && d.close > 0)
                            {
                                d.open = d.close;
                            }
                            if (d.high == 0 && d.close > 0)
                            {
                                d.high = d.close;
                            }
                            if (d.low == 0 && d.close > 0)
                            {
                                d.low = d.close;
                            }
                            return d;
                        }).ToList();

                        break;

                    case instrumentType.HungarianMutualFunds:
                        portfolio_hunfund fund = Db.portfolio_hunfund.Where(s => s.name == instrument).FirstOrDefault();
                        //datasWithPriorData = await PortfolioHelper.GetHungarianMutualFundBetweenDates(fund.ticker, dtFrom, dtTo);
                        throw new Exception("GetHungarianMutualFundBetweenDates not available anymore");
                        break;

                    case instrumentType.HungarianMaxIndexes:
                        portfolio_allampapir allampapir = Db.portfolio_allampapir.Where(s => s.name == instrument).FirstOrDefault();
                        //datasWithPriorData = await PortfolioHelper.GetHungarianAllampapirBetweenDates(allampapir.code, dtFrom, dtTo);
                        throw new Exception("GetHungarianAllampapirBetweenDates not available anymore");
                        break;
                }

                // TODO SPLIT KO 2012-08-01 - 2012-08-20
                datasWithPriorData = datasWithPriorData.Where(d => d.adjusted_close != 0 || d.close != 0)
                    .Select(d => new TIME_SERIES_DAILY_ADJUSTED
                    {
                        adjusted_close = Math.Max(d.adjusted_close, d.close),
                        close = Math.Max(d.adjusted_close, d.close),
                        //adjusted_close = d.adjusted_close,
                        //close = d.close,
                        capitalization = d.capitalization,
                        dividend_amount = d.dividend_amount,
                        high = d.high,
                        low = d.low,
                        open = d.open,
                        timestamp = d.timestamp,
                        volume = d.volume
                    }).ToList();
                if (interval == CandleInterval.Weekly)
                {
                    datasWithPriorData = GenerateWeeklyDataPoints(datasWithPriorData);
                }               
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return datasWithPriorData;
        }

        private List<TIME_SERIES_DAILY_ADJUSTED> GenerateCandlePoints(List<TIME_SERIES_DAILY_ADJUSTED> datasWithPriorData, CandleInterval interval)
        {
            int minutes = 1;
            switch (interval)
            {
                case CandleInterval.Min_1:
                    minutes = 1;
                    break;
                case CandleInterval.Min_5:
                    minutes = 5;
                    break;
                case CandleInterval.Min_15:
                    minutes = 15;
                    break;
                case CandleInterval.Min_30:
                    minutes = 30;
                    break;
                case CandleInterval.Min_60:
                    minutes = 60;
                    break;
            }
            List<TIME_SERIES_DAILY_ADJUSTED> ret = new List<TIME_SERIES_DAILY_ADJUSTED>();
            DateTime dateFrom = datasWithPriorData.Select(r => r.timestamp).Min();
            DateTime dateTo = datasWithPriorData.Select(r => r.timestamp).Max();
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, dateFrom.Hour, dateFrom.Minute, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, dateTo.Hour, dateTo.Minute, 59);
            for (DateTime t1 = dateFrom; t1 < dateTo; t1 = t1.AddMinutes(minutes))
            {
                DateTime t2 = t1.AddMinutes(minutes);
                List<TIME_SERIES_DAILY_ADJUSTED> range = datasWithPriorData.Where(d => d.timestamp >= t1 && d.timestamp < t2).OrderBy(d => d.timestamp).ToList();
                if (range.Any())
                {
                    TIME_SERIES_DAILY_ADJUSTED ohlcPoint = new TIME_SERIES_DAILY_ADJUSTED();
                    ohlcPoint.timestamp = t1;
                    ohlcPoint.open = range.Select(r => r.close).FirstOrDefault();
                    ohlcPoint.high = range.Select(r => r.close).Max();
                    ohlcPoint.low = range.Select(r => r.close).Min();
                    ohlcPoint.close = range.Select(r => r.close).LastOrDefault();
                    ohlcPoint.adjusted_close = range.Select(r => r.close).LastOrDefault();
                    ohlcPoint.volume = range.Select(r => r.volume).Sum();
                    ret.Add(ohlcPoint);
                }
            }
            return ret;
        }

        private List<TIME_SERIES_DAILY_ADJUSTED> GenerateWeeklyDataPoints(List<TIME_SERIES_DAILY_ADJUSTED> datasWithPriorData)
        {
            List<TIME_SERIES_DAILY_ADJUSTED> ret = new List<TIME_SERIES_DAILY_ADJUSTED>();
            var DatasGroupByYear = datasWithPriorData.GroupBy(i => i.timestamp.Year);
            foreach (IGrouping<int, TIME_SERIES_DAILY_ADJUSTED> years in DatasGroupByYear)
            {
                var DatasGroupByWeek = years.GroupBy(i => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(i.timestamp, CalendarWeekRule.FirstDay, DayOfWeek.Monday));
                foreach (IGrouping<int, TIME_SERIES_DAILY_ADJUSTED> days in DatasGroupByWeek)
                {
                    if (!days.Any()) continue;
                    TIME_SERIES_DAILY_ADJUSTED week = new TIME_SERIES_DAILY_ADJUSTED();
                    week.timestamp = days.Select(d => d.timestamp).FirstOrDefault();
                    week.volume = days.Select(d => d.volume).Sum();
                    week.capitalization = days.Select(d => d.capitalization).Last();
                    week.dividend_amount = days.Select(d => d.dividend_amount).Max();
                    week.open = days.Select(d => d.adjusted_close).First();
                    week.adjusted_close = days.Select(d => d.adjusted_close).Last();
                    week.close = days.Select(d => d.close).Last();
                    week.high = days.Select(d => d.high).Max();
                    week.low = days.Select(d => d.low).Min();
                    ret.Add(week);
                }
            }
            return ret;
        }
    }
}
