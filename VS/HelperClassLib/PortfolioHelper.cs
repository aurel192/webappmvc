using DbConnectionClassLib.Data;
using DbConnectionClassLib.Parameters;
using DbConnectionClassLib.ResponseClasses;
using DbConnectionClassLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DbConnectionClassLib.Data.DatabaseInstance;
using static HelperClassLib.Helpers.HelperClass;
#pragma warning disable 4014

namespace HelperClassLib
{
    public static class PortfolioHelper
    {
        private static string GenerateCSV(string response, instrumentType stockType)
        {
            var lines = response.Split('\n').Skip(2).ToArray();
            switch (stockType)
            {
                case instrumentType.HungarianEquities:
                    lines[0] = lines[0].Replace("dátum", "Date")
                                       .Replace("nyitóár", "Open")
                                       .Replace("maximum ár", "High")
                                       .Replace("minimum ár", "Low")
                                       .Replace("záróár", "Close")
                                       .Replace("átlagár", "Avg")
                                       .Replace("forgalom (Ft)", "Volume")
                                       .Replace("forgalom (db)", "VolumeCount");
                    break;
                case instrumentType.HungarianMutualFunds:
                    lines[0] = lines[0].Replace("dátum", "Date")
                                       .Replace("nettóeszközé.", "Capitalization")
                                       .Replace("cashflow", "Cashflow")
                                       .Replace("egy jegyre jutó neé.", "Price")
                                       .Replace("12 hónapos hozam", "YearlyYield");
                    break;
                case instrumentType.HungarianMaxIndexes:
                    lines[0] = lines[0].Replace("dátum", "Date")
                                       .Replace("érték", "Price");
                    break;
            }

            for (int l = 0; l < lines.Count(); l++)
            {
                bool spaceFound = false;
                for (int c = 0; c < lines[l].Count(); c++)
                {
                    if (lines[l][c] == ' ')
                    {
                        if (spaceFound == false)
                        {
                            lines[l] = lines[l].ReplaceAt(c, 1, ";");
                            spaceFound = true;
                        }
                    }
                    else
                    {
                        spaceFound = false;
                    }
                }
                lines[l] = lines[l].Replace(" ", "");
            }
            return string.Join("\n", lines);
        }

        public static async void UpdateHungarianEquity(int ticker, List<PORFOLIO_HUN_EQUITY> newDatas, DateTime? lastDate)
        {
            var DbInst = new ApplicationDbContextFactory().CreateDbContext();
            portfolio_hunstock portfolio_hunstock = null;
            try
            {                
                portfolio_hunstock = DbInst.portfolio_hunstock.Where(s => s.ticker == ticker).FirstOrDefault();
                if (portfolio_hunstock.state == -1) return; // Already updating...
                portfolio_hunstock.state = -1; // Updating flag
                DbInst.SaveChanges();

                IEnumerable<portfolio_hunstock_data> datas = null;

                if (newDatas != null)
                {
                    datas = newDatas.Select(d => new portfolio_hunstock_data
                    {
                        Avg = d.Avg,
                        Close = d.Close,
                        Date = d.Date,
                        High = d.High,
                        Low = d.Low,
                        Open = d.Open,
                        Volume = d.Volume,
                        VolumeCount = (int)d.VolumeCount,
                        StockId = portfolio_hunstock.id
                    });
                }
                else if (lastDate.HasValue)
                {
                    //string csv = await PortfolioHelper.DownloadPlainTextEquityData(ticker.ToString(), lastDate.Value.AddDays(1).ToStr(), DateTime.Today.ToStr());
                    string csv = ""; // TODO
                    csv = PortfolioHelper.GenerateCSV(csv, instrumentType.HungarianEquities);
                    datas = Helper.ParseCSV<PORFOLIO_HUN_EQUITY>(csv, ";").OrderBy(d => d.Date).Select(d => new portfolio_hunstock_data
                    {
                        Avg = d.Avg,
                        Close = d.Close,
                        Date = d.Date,
                        High = d.High,
                        Low = d.Low,
                        Open = d.Open,
                        Volume = d.Volume,
                        VolumeCount = (int)d.VolumeCount,
                        StockId = portfolio_hunstock.id
                    });
                }
                DbInst.portfolio_hunstock_data.AddRange(datas);
                var result = DbInst.SaveChanges();
                portfolio_hunstock.state = result;
                portfolio_hunstock.lastupdate = DateTime.UtcNow;
                DbInst.SaveChanges();
            }
            catch (Exception ex)
            {
                portfolio_hunstock.state = -2; // Error flag -2
                DbInst.SaveChanges();
                throw ex;
            }
        }

        public static async Task<List<TIME_SERIES_DAILY_ADJUSTED>> GetHungarianEquityBetweenDates(int ticker, DateTime dtFrom, DateTime dtTo)
        {
            try
            {
                var instrument = Db.portfolio_hunstock.Where(s => s.ticker == ticker).FirstOrDefault();
                var lastDate = Db.portfolio_hunstock_data.Where(d => d.StockId == instrument.id)
                                                         .OrderByDescending(d => d.Date)
                                                         .Select(d => d.Date).FirstOrDefault();
                bool updated = isUpdated(lastDate, instrumentType.HungarianEquities);
                var data1 = Db.portfolio_hunstock_data.Where(d => d.StockId == instrument.id && d.Date >= dtFrom && d.Date <= dtTo)
                             .OrderBy(d => d.Date)
                             .Select(d => new
                             {
                                timestamp = d.Date,
                                volume = d.VolumeCount,
                                adjusted_close = d.Close,
                                close = d.Close,
                                high = d.High,
                                low = d.Low,
                                open = d.Open
                             }).ToList();
                var data2 = data1.Select(d => new TIME_SERIES_DAILY_ADJUSTED
                {
                    timestamp = d.timestamp,
                    volume = d.volume,
                    adjusted_close = d.adjusted_close,
                    close = d.close,
                    high = d.high,
                    low = d.low,
                    open = d.open
                }).ToList();
                return data2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<List<TIME_SERIES_DAILY_ADJUSTED>> GetHungarianEquityIntraday(string code, bool fromFile = false)
        {
            var result = new List<TIME_SERIES_DAILY_ADJUSTED>();
            
            string url = "https://www.portfolio.hu/tozsde_arfolyamok/BET-" + code.ToUpper() + "-3/" + code.ToLower() + "-napi-koteslista.html";
            string response = "";
            DateTime LastWrite = DateTime.Today;
            if (fromFile)
            {
                string fileName = code.ToUpper() + ".html";
                response = System.IO.File.ReadAllText(fileName);
                System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                LastWrite = fi.LastWriteTime.Date;
            }
            else
            {
                response = await Http.GetBytes(url);
            }
            string startTag = "<tbody>";
            string endTag = "</tbody>";
            var start = response.LastIndexOf(startTag);
            var stop = response.LastIndexOf(endTag);
            string adatok = response.Substring(start + startTag.Length + 1, stop - start - startTag.Length - 1).Replace('-', ' ').Replace('+', ' ').Replace(",", "");
            string[] stringSeparator = new string[] { "<tr class=\"down\">", "<tr class=\"up\">" };
            var lines = adatok.Split(stringSeparator, StringSplitOptions.RemoveEmptyEntries).Where(l => l.Contains("<td>")).ToList();

            DateTime datetime = DateTime.Today;
            foreach (var l in lines)
            {
                List<string> sor = l.Split(new string[] { "<td>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                DateTime.TryParse(sor[1].Substring(0, 8), out datetime);
                string priceStr = sor[3].Replace("</td>", "").Replace("\n", "");
                float price = -1;
                float.TryParse(priceStr, out price);
                string volumeStr = sor[5].Replace("</td>", "").Replace("\n", "");
                double volume = -1;
                double.TryParse(volumeStr, out volume);

                var tsda = new TIME_SERIES_DAILY_ADJUSTED()
                {
                    timestamp = datetime,
                    adjusted_close = price,
                    close = price,
                    volume = (datetime >= new DateTime(datetime.Year, datetime.Month, datetime.Day, 17, 0, 0)) ? 0 : volume
                };

                result.Add(tsda);
            }
            result = result.OrderBy(r => r.timestamp).ToList();
            if (result.Any() && fromFile == false)
            {
                System.IO.File.WriteAllText(code.ToUpper() + ".html", response);
            }
            return result;
        }

        private static bool isUpdated(DateTime lastDate, instrumentType type)
        {
            int maxDaysOlds = 0;           
            switch (type)
            {
                case instrumentType.HungarianEquities:
                    switch (DateTime.Today.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            maxDaysOlds += 1;
                            break;
                        case DayOfWeek.Sunday:
                            maxDaysOlds += 2;
                            break;
                    }
                    break;
                case instrumentType.HungarianMutualFunds:
                    switch (DateTime.Today.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            maxDaysOlds += 2;
                            break;
                        case DayOfWeek.Sunday:
                            maxDaysOlds += 3;
                            break;
                    }
                    break;
                case instrumentType.Crypto:
                    maxDaysOlds = 0;
                    break;
            }
            // TODO: !!! ??? 
            // Portfolion marc17-en legfrissebb adat otpröl marc14. marc15 ünnepnap.
            bool isUpdated = (
                   (lastDate == DateTime.Today) // Utolso adat mai
                     ||
                   (lastDate >= DateTime.Today.AddDays(-maxDaysOlds) && DateTime.Now.TimeOfDay.TotalHours < 20) // Utolso adat tegnapi,vagy tegnapelőtti (bef alap) de azert mert a mai meg nem frissult
                                                                                                                //   || (lastDate.DayOfWeek == DayOfWeek.Friday && (DateTime.Today - lastDate).TotalDays <= 2) // Utolso adat penteki, es 2 napnal nem regebbi
                );
            return isUpdated;
        }

        public static List<BetCsvLine> GetBetCSVLines(string path, DateTime? from = null, string name = null)
        {
            string csvContent = System.IO.File.ReadAllText(path);
            List<string> csvLines = csvContent.Split('\n').ToList();
            csvContent = "NAME,DATE,CLOSE,FORGDB,FORGHUF,FORGEUR,KOTSZAM,OPEN,LOW,HIGH,DEVIZA,AVGPRICE,CAPITALIZATION\n";
            foreach (string csvLine in csvLines)
            {
                if (csvLine.StartsWith("NAME,DATE") || csvLine.StartsWith("Név,Dátum"))
                    continue;
                if (name != null)
                {
                    if (!csvLine.Contains(name))
                        continue;
                }
                if (!csvLine.Contains(",,,"))
                {
                    csvContent += csvLine + "\n";
                }
            }
            List<BetCsvLine> betList = Helper.ParseCSV<BetCsvLine>(csvContent, ",").ToList();
            if (from.HasValue)
                betList = betList.Where(b => b.DATE >= from).ToList();
            return betList;
        }

        public static void UpdatePortfolioHunstockWithBetCSV()
        {
            List<BetCsvLine> betList = PortfolioHelper.GetBetCSVLines(@"C:\--CODE--\webappmvc\ETC\historikus_2022_01_01__2022_01_24_.csv", new DateTime(2022, 01, 12), null);
            List<string> hunStocks = betList.Select(l => l.NAME).Distinct().ToList();
            foreach (string hunStock in hunStocks)
            {
                try
                {
                    portfolio_hunstock reszveny = Db.portfolio_hunstock.Where(p => p.name == hunStock).FirstOrDefault();
                    if (reszveny == null) throw new Exception("hunStock not found");
                    List<PORFOLIO_HUN_EQUITY> list = betList.Where(p => p.NAME == reszveny.name).Select(p => new PORFOLIO_HUN_EQUITY
                    {
                        Date = p.DATE,
                        Close = p.CLOSE,
                        Open = p.OPEN,
                        Low = p.LOW,
                        High = p.HIGH,
                        Volume = p.FORGHUF,
                        VolumeCount = p.FORGDB,
                        Avg = Math.Round(p.AVGPRICE, 2)
                    }).ToList();
                    PortfolioHelper.UpdateHungarianEquity(reszveny.ticker, list, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" ==== HUNSTOCK" + hunStock + "  ___" + ex.Message);
                }
            }
        }

        public static void UpdatePortfolioHunStock()
        {
            List<string> hunStocks = Db.portfolio_hunstock.Select(s => s.name).ToList();
            foreach (string hunStock in hunStocks)
            {
                try
                {
                    portfolio_hunstock reszveny = Db.portfolio_hunstock.Where(p => p.name == hunStock).FirstOrDefault();
                    var line = Db.portfolio_hunstock_data.Where(d => d.StockId == reszveny.id && d.Date == DateTime.Today).FirstOrDefault();
                    if (line != null)
                        continue;
                    List<TIME_SERIES_DAILY_ADJUSTED> datas = null;
                    try
                    {
                        datas = PortfolioHelper.GetHungarianEquityIntraday(hunStock).Result;
                    }
                    catch { continue; }
                    PORFOLIO_HUN_EQUITY todayData = new PORFOLIO_HUN_EQUITY();
                    if (datas == null || datas.Any() == false)
                        continue;
                    todayData.Open = datas.OrderBy(d => d.timestamp).Select(d => d.close).FirstOrDefault();
                    todayData.Close = datas.OrderBy(d => d.timestamp).Select(d => d.close).LastOrDefault();
                    todayData.Avg = Math.Round((double)datas.Select(d => d.close).Average(), 2);
                    todayData.Date = DateTime.Today;
                    todayData.High = datas.Select(d => d.close).Max();
                    todayData.Low = datas.Select(d => d.close).Min();
                    todayData.Volume = datas.Select(d => d.capitalization).Sum();
                    todayData.VolumeCount = datas.Select(d => d.volume).Sum();
                    var lista = new List<PORFOLIO_HUN_EQUITY>() { todayData };
                    var ExistingDates = Db.portfolio_hunstock_data.Where(d => d.StockId == reszveny.id && d.Date == todayData.Date).Select(d => d.Date).ToList();
                    lista = lista.Where(d => !ExistingDates.Contains(d.Date)).ToList();
                    if (lista.Any())
                    {
                        PortfolioHelper.UpdateHungarianEquity(reszveny.ticker, lista, null);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" ****** EXCEPTION ConsoleApp UpdatePortfolio() " + ex);
                    Exception ConsoleAppException = new Exception("EXCEPTION ConsoleApp UpdatePortfolio(" + hunStock + ") " + DateTime.Now.ToDateTimeString(), ex);
                    Helper.LogException(ConsoleAppException);
                    //throw ex;
                }
            }
        }
    }
}
#pragma warning restore 4014

/*
        private async static Task<string> DownloadPlainTextEquityData(string ticker, string from, string to)
        {
            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();
            kvpList.Add(new KeyValuePair<string, string>("tipus", "1"));
            kvpList.Add(new KeyValuePair<string, string>("startdate", from));
            kvpList.Add(new KeyValuePair<string, string>("enddate", to));
            kvpList.Add(new KeyValuePair<string, string>("open", "1"));
            kvpList.Add(new KeyValuePair<string, string>("max", "1"));
            kvpList.Add(new KeyValuePair<string, string>("avg", "1"));
            kvpList.Add(new KeyValuePair<string, string>("min", "1"));
            kvpList.Add(new KeyValuePair<string, string>("close", "1"));
            kvpList.Add(new KeyValuePair<string, string>("forg", "1"));
            kvpList.Add(new KeyValuePair<string, string>("forgdb", "1"));
            kvpList.Add(new KeyValuePair<string, string>("ticker", ticker + ":" + from + ":" + to));
            kvpList.Add(new KeyValuePair<string, string>("text", "szövegfájl"));
            string response = await Http.Post("http://www.portfolio.hu", "history/reszveny-adatok.php", kvpList);
            return response;
        }
*/