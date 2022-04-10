using DbConnectionClassLib.ResponseClasses;
using DbConnectionClassLib.Tables;
using HelperClassLib;
using HelperClassLib.AlphaVantage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DbConnectionClassLib.Data.DatabaseInstance;
using DbInstance = DbConnectionClassLib.Data.DatabaseInstance;
using static HelperClassLib.Helpers.HelperClass;
using System.IO;

namespace ConsoleApp
{
    public static class ConsoleHelper
    {
        public async static void GetSymbols()
        {
            // QG-töl
            List<char> alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
            int delay = 12050;
            int errors = 0;
            foreach (var firstLetter in alphabet)
            {
            tryagain:
                if (errors == 100)
                {
                    return;
                }
                try
                {
                    SearchSymbol(firstLetter.ToString());
                }
                catch
                {
                    Console.WriteLine("Errors: " + (++errors));
                    goto tryagain;
                }
                await Task.Delay(delay);
                foreach (var secondLetter in alphabet)
                {
                    string search = firstLetter.ToString() + secondLetter.ToString();
                    try
                    {
                        SearchSymbol(search);
                    }
                    catch
                    {
                        Console.WriteLine("Errors: " + (++errors));
                        goto tryagain;
                    }
                    await Task.Delay(delay);
                }
            }
        }

        public static void SearchSymbol(string search)
        {
            Console.WriteLine("\n================ " + search + " ============");
            List<AV_SYMBOL_SEARCH_RESPONSE> results = null;
            try
            {
                AlphaVantageClient avc = new AlphaVantageClient(new AlphaVantageSymbolSearchParameters(search));
                results = avc.GetDataSymbolSearch();
                foreach (AV_SYMBOL_SEARCH_RESPONSE r in results)
                {
                    Console.WriteLine(r.ToString());
                    AV_SYMBOL symbol = new AV_SYMBOL
                    {
                        name = r.name,
                        symbol = r.symbol,
                        region = r.region,
                        currency = r.currency,
                        marketClose = r.marketClose,
                        marketOpen = r.marketOpen,
                        timezone = r.timezone,
                        type = r.type
                    };
                    if (Db.av_symbols.Where(s => s.symbol == symbol.symbol).Any() == false)
                    {
                        Db.av_symbols.Add(symbol);
                        Db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n\n EXCEPTION " + ex.Message + " ============");
                HelperClassLib.Helpers.HelperClass.Helper.LogException(ex);
            }
        }

        public static void ListingStatus()
        {
            Console.WriteLine("\n================ LISTING STATUS ============");
            List<AV_LISTING> results = null;
            try
            {
                AlphaVantageClient avc = new AlphaVantageClient(new AlphaVantageListingParameters());
                results = avc.GetDataListingStatus();
                foreach (AV_LISTING item in results)
                {
                    Console.WriteLine(item.ToString() + "\n______\n");
                    AV_TIME_SERIES_DAILY_ADJUSTED avtsda = new AV_TIME_SERIES_DAILY_ADJUSTED
                    {
                        name = item.name,
                        symbol = item.symbol,
                        lastupdate = null,
                        state = 0
                    };
                    if (Db.av_time_series_daily_adjusted.Where(s => s.symbol == item.symbol).Any() == false)
                    {
                        Db.av_time_series_daily_adjusted.Add(avtsda);
                        Db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n\n EXCEPTION " + ex.Message + " ============");
                HelperClassLib.Helpers.HelperClass.Helper.LogException(ex);
            }
        }

        public static void MultiCondTest()
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            dict.Add("A", false);
            dict.Add("B", false);
            dict.Add("C", true);
            dict.Add("D", true);
            string condition = "(A OR B) OR NOT ( C AND NOT D )";
            var r = MultipleConditions.Evaluate(dict, condition);
            Console.WriteLine(condition + " result: " + r);
        }

        public static void Update_Index()
        {
            string filecontent = HelperClassLib.Helpers.HelperClass.Helper.ReadStringFromFile(@"c:\Users\aurel\Downloads\INDEX.txt");
            var lines = filecontent.Split('\n');
            foreach (var line in lines)
            {
                Console.WriteLine(line);
                var firstTab = line.IndexOf('\t');
                if (firstTab == -1)
                    continue;
                var symbol = line.Substring(0, firstTab);
                if (symbol == "Symbol")
                    continue;
                var name = line.Substring(firstTab + 1, line.Length - firstTab - 1);
                Console.WriteLine("symbol='" + symbol + "'    name='" + name + "'");
                Db.av_time_series_daily_adjusted.Add(new DbConnectionClassLib.Tables.AV_TIME_SERIES_DAILY_ADJUSTED()
                {
                    name = name,
                    symbol = symbol
                });
            }
            Db.SaveChanges();
        }

        public static void UpdateGitRevision()
        {
            try
            {
                string commit = GetCurrentGitRevision();
                if (string.IsNullOrEmpty(commit)) return;
                var SETTING_COMMIT = DbInstance.Db.db_settings.Where(s => s.Key == "COMMIT").FirstOrDefault();
                if (SETTING_COMMIT == null)
                {
                    SETTING_COMMIT = new DBSettings
                    {
                        Key = "COMMIT",
                        Value = commit
                    };
                    DbInstance.Db.db_settings.Add(SETTING_COMMIT);
                }
                else
                {
                    SETTING_COMMIT.Value = commit;
                }
                DbInstance.Db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Exception ConsoleAppException = new Exception(" ****** EXCEPTION ConsoleApp UpdateGitRevision() " + DateTime.Now.ToDateTimeString(), ex);
                Helper.LogException(ConsoleAppException);
                //throw ex;
            }
        }

        private static string GetCurrentGitRevision()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                path = path.Replace("VS\\ConsoleApp\\bin\\Debug\\netcoreapp2.0", "") + ".git\\logs\\HEAD";
                string filecontent = Helper.ReadStringFromFile(path);
                string lastLine = filecontent.Split("\n")
                                             .Where(l => !string.IsNullOrEmpty(l) && l.Length > 10)
                                             .LastOrDefault();
                var x = lastLine;
                int firstIndexOfSpace = lastLine.IndexOf(" ");
                int secondIndexOfSpace = lastLine.IndexOf(" ", firstIndexOfSpace + 1);
                string commit = lastLine.Substring(firstIndexOfSpace + 1, secondIndexOfSpace - firstIndexOfSpace - 1);
                Console.WriteLine("  ==== COMMIT NUMBER: " + commit);
                return commit;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static double Log(double _base, double x)
        {
            //plot (log(4,x)+10)/10
            double y = (Math.Log(_base, x + 1));
            return y;
        }

        private static double Exponential(double a, double x)
        {
            double y = (Math.Pow(a, x) - 1) / (a - 1);
            return y;
        }

        private static void ExponentialTest()
        {
            //for (double i = 0; i < 100; i+=5)
            //{
            //    double exp = Exponential(i, 2);
            //    Console.WriteLine("i=" + i + "   exp=" + exp);
            //}
            for (double x = -1; x < 20; x += 0.5)
            {
                double y = Log(2, x);
                Console.WriteLine("y=" + x.ToString("0.##") + "   x=" + y.ToString("0.##"));
            }
        }
    }
}
