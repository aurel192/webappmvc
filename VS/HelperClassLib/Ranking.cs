using DbConnectionClassLib.Parameters;
using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HelperClassLib.Helpers.HelperClass;
using static DbConnectionClassLib.Data.DatabaseInstance;

namespace HelperClassLib
{
    public static class Ranking
    {
        public static void Otp()
        {
            try
            {
                CompareInstrumentsParam parameter = new CompareInstrumentsParam() { datefrom = new DateTime(1970, 1, 1), dateto = DateTime.Now };
                parameter.formData = Db.portfolio_hunfund.Where(hf => hf.name.StartsWith("OTP"))
                                                         .Select(o => new FormElement
                                                         {
                                                             stock = o.id,
                                                             text = o.name,
                                                             type = (int)instrumentType.HungarianMutualFunds
                                                         }).ToList();
                CreateRankingList(parameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void CreateRankingList(CompareInstrumentsParam p)
        {
            try
            {
                List<RankingItem> rankings = new List<RankingItem>();
                List<TSDACListItem> list = new List<TSDACListItem>();
                double darab = p.formData.Count;
                double cnt = 0;

                foreach (FormElement s in p.formData)
                {
                    Console.WriteLine("======  " + s.text + "   ======");
                    Console.WriteLine(Math.Round((double)(cnt * 100 / darab), 0) + " %");
                    //var datas = Helper.GetInstrumentDataBetweenDates(parameter.datefrom, parameter.dateto, (instrumentType)s.type, s.stock, s.text).Result;
                    var datas = Helper.GetInstrumentDataBetweenDates(p.datefrom, p.dateto, (instrumentType)s.type, s.text).Result;
                    RankingItem r = CreateRanking(datas, s.stock, s.text, (instrumentType)s.type);
                    rankings.Add(r);
                    cnt++;
                }

                Helper.Logger("\n\n         ORDERED BY AVG YEARLY YIELD\n\n");
                PrintResults(rankings.OrderByDescending(r => r.avgYearlyYield).ToList());

                Helper.Logger("\n\n         ORDERED BY YEARLY YIELD\n\n");
                PrintResults(rankings.OrderByDescending(r => r.yearlyYield).ToList());

                Helper.Logger("\n\n         ORDERED BY CAPITALIZATION\n\n");
                PrintResults(rankings.OrderByDescending(r => r.capitalization).ToList());

                Helper.Logger("\n\n         ORDERED BY MAX DROP PERCENT DESC\n\n");
                PrintResults(rankings.OrderBy(r => r.maxDropPercent).ToList());

                Helper.Logger("\n\n         ORDERED BY MAX DAYS WITHOUT RISE DESC\n\n");
                PrintResults(rankings.OrderBy(r => r.maxDaysWithoutRise).ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static RankingItem CreateRanking(List<TIME_SERIES_DAILY_ADJUSTED> datas, int stock, string text, instrumentType type)
        {
            try
            {
                DateTime fromDate = datas.OrderBy(o => o.timestamp).FirstOrDefault().timestamp;
                DateTime toDate = datas.OrderByDescending(o => o.timestamp).FirstOrDefault().timestamp;
                VirtualMonthlyInvestment(datas, 50000, new DateTime(2010, 1, 1));
                //return new RankingItem();
                List<YieldInYear> yearlyYields = CalculateYearlyYields(datas);
                double yieldPercent = Math.Round(((double)(datas.Last().adjusted_close / datas.FirstOrDefault().adjusted_close) * 100), 2);
                double avgYearlyYield = Math.Round(yearlyYields.Select(yield => yield.percent).Sum() / yearlyYields.Count, 2);
                double yearlyYield = CalculateYield(datas.FirstOrDefault().adjusted_close, datas.Last().adjusted_close, (toDate - fromDate).Days);
                var DropList = CalculateMaxDropPercent(datas, 5);
                double maxDropPercent = DropList.Any() ? DropList.First().percent : 0;
                var monthlyYields = CalculateMonthlyYields(datas);
                var DaysWithoutRise = CalculateDaysWithoutRise(datas);
                int maxDaysWithoutRise = DaysWithoutRise.Any() ? DaysWithoutRise.First().days : 0;
                return new RankingItem
                {
                    alapid = stock,
                    name = text,
                    type = type,
                    capitalization = datas.OrderByDescending(o => o.timestamp).FirstOrDefault().capitalization,
                    fromDate = fromDate,
                    toDate = toDate,
                    days = (toDate - fromDate).Days,
                    yearlyYields = yearlyYields,
                    yieldPercent = yieldPercent,
                    avgYearlyYield = avgYearlyYield,
                    yearlyYield = yearlyYield,
                    monthlyYields = monthlyYields,
                    maxDropPercent = maxDropPercent,
                    DropList = DropList,
                    DaysWithoutRise = DaysWithoutRise,
                    maxDaysWithoutRise = maxDaysWithoutRise
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void VirtualMonthlyInvestment(List<TIME_SERIES_DAILY_ADJUSTED> datas, double invest, DateTime dtFrom, double fee = 250)
        {
            int own = 0;
            double sumInvest = 0;
            for (DateTime date = dtFrom; date < DateTime.Today; date = date.AddMonths(1))
            {
                var buy = datas.Where(d => d.timestamp.Year == date.Year && d.timestamp.Month == date.Month).FirstOrDefault();
                if (buy != null)
                {
                    own += (int)((invest - fee) / buy.adjusted_close);
                    sumInvest += invest;
                }
            }
            Console.WriteLine(own.ToString("N") + " darab");
            Console.WriteLine("sumInvested: " + sumInvest.ToString("N") + " Ft");
            var last = datas.Last();
            Console.WriteLine("currentPrice: " + (own * last.adjusted_close).ToString("N") + " Ft");
            Console.WriteLine((Math.Round(100 * ((own * last.adjusted_close) / sumInvest), 2)).ToString("N") + " %");
        }

        private static List<DaysWithoutRise> CalculateDaysWithoutRise(List<TIME_SERIES_DAILY_ADJUSTED> datas, int num = 5)
        {
            List<DaysWithoutRise> DaysWithoutRise = new List<DaysWithoutRise>();
            foreach (var curr in datas)
            {
                var higher = datas.Where(d => d.timestamp > curr.timestamp && d.adjusted_close > curr.adjusted_close).OrderBy(l => l.timestamp).FirstOrDefault();
                var dwr = new DaysWithoutRise() { fromDate = curr.timestamp };
                if (higher != null)
                {
                    var lowest = datas.Where(d => d.timestamp > curr.timestamp && d.timestamp < higher.timestamp).OrderBy(l => l.adjusted_close).FirstOrDefault();
                    dwr.toDate = higher.timestamp;
                    dwr.days = (higher.timestamp - curr.timestamp).Days;
                    dwr.dropPercent = (lowest != null) ? Math.Round((1 - (lowest.adjusted_close / curr.adjusted_close)) * 100, 2) : 0;
                }
                else
                {
                    dwr.toDate = DateTime.Today;
                    dwr.days = (DateTime.Today - curr.timestamp).Days;
                    dwr.dropPercent = Math.Round((1 - (datas.Last().adjusted_close / curr.adjusted_close)) * 100, 2);
                }
                bool add = true;
                foreach (var exDwr in DaysWithoutRise)
                {
                    if (dwr.fromDate.InRange(exDwr.fromDate, exDwr.toDate))
                    {
                        add = false;
                        break;
                    }
                    if (dwr.toDate.InRange(exDwr.fromDate, exDwr.toDate))
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                {
                    DaysWithoutRise.Add(dwr);
                    DaysWithoutRise = DaysWithoutRise.OrderByDescending(d => d.days).Take(num).ToList();
                }
            }
            return DaysWithoutRise;
        }

        private static List<YieldInMonth> CalculateMonthlyYields(List<TIME_SERIES_DAILY_ADJUSTED> datas)
        {
            List<YieldInMonth> months = new List<YieldInMonth>();
            var fromMonth = datas.FirstOrDefault().timestamp;
            var toMonth = datas.Last().timestamp;
            fromMonth = new DateTime(fromMonth.Year, fromMonth.Month, 1);
            toMonth = new DateTime(toMonth.Year, toMonth.Month, 1);
            for (DateTime month = fromMonth; month < toMonth; month = month.AddMonths(1))
            {
                YieldInMonth yim = new YieldInMonth(month);
                // Legelso honap
                if (datas.FirstOrDefault().timestamp.Year == month.Year && datas.FirstOrDefault().timestamp.Month == month.Month)
                {
                    var firstInMonth = datas.Where(d => d.timestamp.Year == month.Year && d.timestamp.Month == month.Month).OrderBy(d => d.timestamp).FirstOrDefault();
                    var lastInMonth = datas.Where(d => d.timestamp.Year == month.Year && d.timestamp.Month == month.Month).OrderByDescending(d => d.timestamp).FirstOrDefault();
                    if (firstInMonth == null || lastInMonth == null) continue;
                    yim.percent = Math.Round((((lastInMonth.adjusted_close / firstInMonth.adjusted_close) - 1) * 100), 2);
                    months.Add(yim);
                }
                else
                {
                    var lastInLastMonth = datas.Where(d => d.timestamp.Year == month.AddMonths(-1).Year && d.timestamp.Month == month.AddMonths(-1).Month).OrderByDescending(d => d.timestamp).FirstOrDefault();
                    var lastInMonth = datas.Where(d => d.timestamp.Year == month.Year && d.timestamp.Month == month.Month).OrderByDescending(d => d.timestamp).FirstOrDefault();
                    if (lastInLastMonth == null || lastInMonth == null) continue;
                    yim.percent = Math.Round((((lastInMonth.adjusted_close / lastInLastMonth.adjusted_close) - 1) * 100), 2);
                    months.Add(yim);
                }
            }
            return months;
        }

        private static void PrintResults(List<RankingItem> rankings)
        {
            string result = "";
            foreach (var r in rankings)
            {
                result += "\n_________________________  " + r.name + "  ___________________________________";
                result += "\n                           " + r.fromDate.ToStr() + "  -  " + r.toDate.ToStr() + "\n";
                result += "Yield: " + r.yieldPercent + " %   in  " + r.days + " days\n";                
                result += "Yearly yield: " + r.yearlyYield + " %\n";
                string cap = "";
                if ((r.capitalization / 1000000000) > 1)
                    cap = Math.Round((r.capitalization / 1000000000), 0).ToString() + " Mrd.";
                else if ((r.capitalization / 1000000) > 1)
                    cap = Math.Round((r.capitalization / 1000000), 0).ToString() + " M.";
                else
                    cap = r.capitalization.ToString("N").Replace(",00", "");
                result += "Capitalization: " + cap + "\n";
                foreach (var yy in r.yearlyYields)
                {
                    result += "Yield in " + yy.year + ": " + yy.percent + " %\n";
                }
                result += "Avg. yearly yield: " + r.avgYearlyYield + " %\n";
                /*
                foreach (var m in r.monthlyYields)
                {
                    result += "Yield in " + m.month.Year + "." + m.month.Month.ToString("D2") + ": " + m.percent + " %\n";
                }
                */
                foreach (var d in r.DropList)
                {
                    result += "Dropped   " + d.percent + " %  " + d.fromDate.ToStr() + " - " + d.toDate.ToStr() + " in " + (d.toDate - d.fromDate).Days + " days\n";
                }
                foreach (var d in r.DaysWithoutRise)
                {
                    result += "DaysWithoutRise   " + d.days + " days  " + d.fromDate.ToStr() + " - " + d.toDate.ToStr() + "  MaxDrop: " + d.dropPercent + " %\n";
                }
            }
            Helper.Logger(result);
        }

        private static List<Drop> CalculateMaxDropPercent(List<TIME_SERIES_DAILY_ADJUSTED> list, int num = 5)
        {
            List<Drop> GreatestDrops = new List<Drop>();
            foreach (var p in list)
            {
                DateTime after = DateTime.MinValue;
                var GreatestDropDates = GreatestDrops.Select(gd => gd.toDate).ToList();
                if (GreatestDropDates.Any())
                    after = GreatestDropDates.Max();
                if (p.timestamp < after) continue;

                TIME_SERIES_DAILY_ADJUSTED lowest = list.Where(l => l.timestamp >= p.timestamp).OrderBy(l => l.adjusted_close).FirstOrDefault();
                TIME_SERIES_DAILY_ADJUSTED highest = list.Where(l => l.timestamp > p.timestamp && l.timestamp < lowest.timestamp).OrderByDescending(l => l.adjusted_close).FirstOrDefault();

                if (lowest == null || highest == null) continue;
                var dropPercent = 100 - ((lowest.adjusted_close / highest.adjusted_close) * 100);
                TIME_SERIES_DAILY_ADJUSTED recovered = list.Where(l => l.timestamp > lowest.timestamp && l.adjusted_close >= highest.adjusted_close).OrderBy(l => l.timestamp).FirstOrDefault();
                var drop = new Drop { fromDate = highest.timestamp, toDate = lowest.timestamp, percent = Math.Round(dropPercent, 2) };
                if (recovered != null)
                {
                    drop.Recovered = recovered.timestamp;
                    drop.RecoveryDays = (recovered.timestamp - lowest.timestamp).Days;
                }
                GreatestDrops.Add(drop);
                GreatestDrops = GreatestDrops.OrderByDescending(d => d.percent).Take(num).ToList();
            }
            return GreatestDrops;
        }

        private static double CalculateYield(double buy, double sell, double days)
        {
            return Math.Round((Math.Pow((sell / buy), ((double)365 / days)) - 1) * 100, 2);
        }

        private static List<YieldInYear> CalculateYearlyYields(List<TIME_SERIES_DAILY_ADJUSTED> datas)
        {
            List<YieldInYear> yearlyYields = new List<YieldInYear>();
            try
            {
                int fromYear = datas.FirstOrDefault().timestamp.Year;
                int toYear = datas.Last().timestamp.Year;
                for (int year = fromYear; year < toYear; year++)
                {
                    var firstInYear = datas.Where(d => d.timestamp.Year == year).OrderBy(d => d.timestamp).FirstOrDefault();
                    var lastInYear = datas.Where(d => d.timestamp.Year == year).OrderByDescending(d => d.timestamp).FirstOrDefault();
                    if (firstInYear != null && lastInYear != null)
                    {
                        var yearlyYield = Math.Round(((double)(lastInYear.adjusted_close / firstInYear.adjusted_close) * 100) - 100, 2);
                        yearlyYields.Add(new YieldInYear() { year = year, percent = yearlyYield });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return yearlyYields;
        }
    }
}
