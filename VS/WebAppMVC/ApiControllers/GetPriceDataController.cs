using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbConnectionClassLib.Data;
using DbConnectionClassLib.Parameters;
using DbConnectionClassLib.ResponseClasses;
using HelperClassLib;
using HelperClassLib.AlphaVantage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppMVC.Services;
using static HelperClassLib.Helpers.HelperClass;

namespace WebAppMVC.ApiControllers
{
    [Produces("application/json")]
    [Route("api/GetPriceData")]
    public class GetPriceDataController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext db;

        public GetPriceDataController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<GetPriceDataController> logger)
        {
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ChartResponse> Post(GetPriceDataParameter p)
        {
            ChartResponse response = null;
            try
            {
                DateTime ReqStart = DateTime.Now;
                DateTime dtFrom = p.startDateStr.ToDate(DateTime.MinValue);
                DateTime dtTo = p.endDateStr.ToDate(DateTime.MaxValue);
                if (dtFrom == dtTo)
                {
                    dtFrom = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day, 0, 0, 0);
                    dtTo = new DateTime(dtTo.Year, dtTo.Month, dtTo.Day, 23,59,59);
                }
                var tmp = p.instrumentFullPath.Split('/').ToList();
                var instrument = tmp.Last();
                var instrtype = tmp.FirstOrDefault();
                List<TIME_SERIES_DAILY_ADJUSTED> datasWithPriorData = null;
                response = new ChartResponse(p.instrumentName, p.instrumentName, dtFrom, dtTo);
                int _maxOffset = (new List<int>() { p.rsi, p.macdfast, p.macdslow, p.macdsignal, 100 }).Max() * 3;
                CandleInterval interval = CandleInterval.Daily;
                if ((dtTo - dtFrom).Days > 365 * 2)
                {
                    interval = CandleInterval.Weekly;
                    _maxOffset = 200 * 7 + 30;
                }
                else if (dtFrom.Date == dtTo.Date)
                {
                    switch (p.interval)
                    {
                        case "1min":
                            interval = CandleInterval.Min_1;
                            break;
                        case "5min":
                            interval = CandleInterval.Min_5;
                            break;
                        case "15min":
                            interval = CandleInterval.Min_15;
                            break;
                        case "30min":
                            interval = CandleInterval.Min_30;
                            break;
                        case "60min":
                            interval = CandleInterval.Min_60;
                            break;
                    }
                }
                DateTime dtFromPrior = dtFrom.AddDays(-1 * _maxOffset);
                if (interval != CandleInterval.Daily && interval != CandleInterval.Weekly && !p.instrumentFullPath.Contains("Hungarian Equities"))
                {
                    dtFrom = DateTime.MinValue;
                    dtTo = DateTime.MaxValue;
                }

                switch (instrtype)
                {
                    case "Otp Funds":
                        break;
                    case "Equities":
                        datasWithPriorData = await Helper.GetInstrumentDataBetweenDates(dtFromPrior, dtTo, instrumentType.Equities, instrument, p.instrumentName, interval);
                        response.VolumeData.points = Helper.getVolumeChangeData(datasWithPriorData, dtFrom, dtTo);
                        response.overview = AlphaVantageHelper.GetAndUpdateOverview(instrument);
                        break;
                    case "Indexes":
                        break;
                    case "Forex":
                        break;
                    case "Crypto":
                        break;
                    case "Hungarian Equities":
                        if (dtFrom.Date == dtTo.Date)
                        {
                            datasWithPriorData = await Helper.GetInstrumentDataBetweenDates(dtFrom.Date, dtTo.Date, instrumentType.HungarianEquitiesIntraday, instrument, null, interval);
                            response.VolumeData.points = Helper.getVolumeChangeData(datasWithPriorData, dtFrom, dtTo);
                        }
                        else
                        {
                            datasWithPriorData = await Helper.GetInstrumentDataBetweenDates(dtFromPrior, dtTo, instrumentType.HungarianEquities, instrument, null, interval);
                            response.VolumeData.points = Helper.getVolumeChangeData(datasWithPriorData, dtFrom, dtTo);
                        }
                        break;
                    case "Hungarian Mutual Funds":
                        datasWithPriorData = await Helper.GetInstrumentDataBetweenDates(dtFromPrior, dtTo, instrumentType.HungarianMutualFunds, instrument, null, interval);
                        response.VolumeData.points = Helper.CalculateVolumeDataFromCapitalization(datasWithPriorData, dtFrom, dtTo);
                        break;
                    case "Hungarian Equities (BÉT)":
                        break;
                    case "Commodities":
                        break;
                    case "Hungarian MAX Indexes":
                        datasWithPriorData = await Helper.GetInstrumentDataBetweenDates(dtFromPrior, dtTo, instrumentType.HungarianMaxIndexes, instrument, null, interval);
                        response.VolumeData.points = Helper.getVolumeChangeData(datasWithPriorData, dtFrom, dtTo);
                        break;
                }

                if (dtFrom.Date != dtTo.Date && !instrtype.StartsWith("Hungarian"))
                    response.VolumeData.points = response.VolumeData.points.Select(vd => vd / 1000000).ToList();

                List<TIME_SERIES_DAILY_ADJUSTED> datas = new List<TIME_SERIES_DAILY_ADJUSTED>();

                datas = datasWithPriorData.Where(d => d.timestamp >= dtFrom).ToList();
                List<DateTimeDoublePair> datasWithPriorDataAsDateTimeDoublePair = datasWithPriorData.Select(d => new DateTimeDoublePair { date = d.timestamp, value = d.adjusted_close }).ToList();
                if (!(datasWithPriorDataAsDateTimeDoublePair.Count > _maxOffset)) {
                    response.comment = "if (!(datasWithPriorDataAsDateTimeDoublePair.Count > _maxOffset)) {";
                    //p.rsi = 0;
                    //p.macdsignal = 0;
                }
                if (dtFrom.Date == dtTo.Date)
                {
                    response.DateTimeData.points = datas.Where(d => d.timestamp.Date == dtFrom).Select(d => d.timestamp).ToList();
                    response.PriceData.points = datas.Where(d => d.timestamp.Date == dtFrom).Select(d => d.adjusted_close).ToList();
                }
                else
                {
                    response.DateTimeData.points = datas.Where(d => d.timestamp >= dtFrom && d.timestamp <= dtTo).Select(d => d.timestamp).ToList();
                    response.PriceData.points = datas.Where(d => d.timestamp >= dtFrom && d.timestamp <= dtTo).Select(d => d.adjusted_close).ToList();
                }
                
                if (!response.PriceData.points.Any())
                    throw new Exception("NO DATAPOINTS FOUND");

                if (datas.Where(d => d.high > 0 && d.low > 0 && d.open > 0 && d.close > 0).Any())
                    response.PriceData.ohlcpoints = datas.Where(d => d.timestamp >= dtFrom && d.timestamp <= dtTo).Select(d => new OHLC { open = d.open, close = d.close, high = d.high, low = d.low }).ToList();
                response.CapitalizationData.points = datas.Where(d => d.timestamp >= dtFrom && d.timestamp <= dtTo).Select(d => d.capitalization / 1000000).ToList();
                response.PercentData.points = Helper.getPercentData(response.PriceData.points);

                response.PriceData.change = Helper.GetChangeValue(response.PriceData.ohlcpoints);
                response.PriceData.changebeforeopen = Helper.GetChangeValue(response.PriceData.ohlcpoints, true);

                // Technical Indicators
                List<Signal> rsiSignals = new List<Signal>();
                List<Signal> macdSignals = new List<Signal>();
                int rsiBeginIndex = -1;
                int macdBeginIndex = -1;
                if (p.rsi > 0)
                {
                    var rsiPoints = TechnicalAnalysis.RSI(datasWithPriorDataAsDateTimeDoublePair, out rsiBeginIndex, p.rsi)
                                                     .Where(d => d.date >= dtFrom && d.date <= dtTo).ToList();
                    response.RsiData.points = rsiPoints.Select(r => r.rsi).ToList();

                    rsiSignals = Signals.GetRSISignals(rsiPoints);
                }

                if (p.macdfast > 0 && p.macdslow > 0 && p.macdsignal > 0)
                {
                    List<MACDValues> macdpoints = TechnicalAnalysis.MACD(datasWithPriorDataAsDateTimeDoublePair, out macdBeginIndex, p.macdfast, p.macdslow, p.macdsignal)
                                                                   .Where(d => d.date >= dtFrom && d.date <= dtTo).ToList();
                    response.MacdData.Macd.points = macdpoints.Select(m => m.macd).ToList();
                    if (response.MacdData.Macd.points.Any())
                    {
                        response.MacdData.MacdHistogram.points = macdpoints.Select(m => m.hist).ToList();
                        response.MacdData.MacdSignal.points = macdpoints.Select(m => m.signal).ToList();


                        response.MacdData.Macd.min = response.MacdData.Macd.points.Min();
                        response.MacdData.Macd.max = response.MacdData.Macd.points.Max();
                        response.MacdData.Macd.min = new List<double> { Math.Abs(response.MacdData.Macd.min), response.MacdData.Macd.max }.Max() * -1;
                        response.MacdData.Macd.max = new List<double> { Math.Abs(response.MacdData.Macd.min), response.MacdData.Macd.max }.Max();
                        if (response.MacdData.Macd.min == 0 && response.MacdData.Macd.max == 0)
                        {
                            response.MacdData.Macd.min = -1;
                            response.MacdData.Macd.max = 1;
                        }

                        response.MacdData.MacdHistogram.min = response.MacdData.MacdHistogram.points.Min();
                        response.MacdData.MacdHistogram.max = response.MacdData.MacdHistogram.points.Max();
                        response.MacdData.MacdHistogram.min = new List<double> { Math.Abs(response.MacdData.MacdHistogram.min), response.MacdData.MacdHistogram.max }.Max() * -1;
                        response.MacdData.MacdHistogram.max = new List<double> { Math.Abs(response.MacdData.MacdHistogram.min), response.MacdData.MacdHistogram.max }.Max();
                        if (response.MacdData.MacdHistogram.min == 0 && response.MacdData.MacdHistogram.max == 0)
                        {
                            response.MacdData.MacdHistogram.min = -1;
                            response.MacdData.MacdHistogram.max = 1;
                        }
                        macdSignals = Signals.GetMACDSignals(macdpoints, p.macdsignal);

                        response.MacdData.MacdSignal.min = response.MacdData.MacdSignal.points.Min();
                        response.MacdData.MacdSignal.max = response.MacdData.MacdSignal.points.Max();
                        response.MacdData.MacdSignal.min = new List<double> { Math.Abs(response.MacdData.MacdSignal.min), response.MacdData.MacdSignal.max }.Max() * -1;
                        response.MacdData.MacdSignal.max = new List<double> { Math.Abs(response.MacdData.MacdSignal.min), response.MacdData.MacdSignal.max }.Max();
                        if (response.MacdData.MacdSignal.min == 0 && response.MacdData.MacdSignal.max == 0)
                        {
                            response.MacdData.MacdSignal.min = -1;
                            response.MacdData.MacdSignal.max = 1;
                        }
                    }
                }
                if (p.bbandperiod > 0)
                {
                    var bbandpoints = TechnicalAnalysis.BBands(datasWithPriorDataAsDateTimeDoublePair, p.bbandperiod, p.bbandup, p.bbanddown);
                    bbandpoints = bbandpoints.Where(d => d.date >= dtFrom && d.date <= dtTo).ToList();
                    response.BbandsData.lower.points = bbandpoints.Select(b => b.lower).ToList();
                    response.BbandsData.middle.points = bbandpoints.Select(b => b.middle).ToList();
                    response.BbandsData.upper.points = bbandpoints.Select(b => b.upper).ToList();
                }
                
                if (true) // TODO: MovingAvarage Paramters
                {
                    int ma1BeginIndex = -1;
                    int ma2BeginIndex = -1;
                    int ma1 = 50;
                    int ma2 = 200;
                    if (instrtype == "Hungarian Equities" && dtFrom.Date == dtTo.Date) // magyar intraday
                    {

                    }
                    else
                    {
                        var ma1Points = HelperClassLib.Helpers.Indicators.MA(datasWithPriorDataAsDateTimeDoublePair, out ma1BeginIndex, ma1);
                        var ma2Points = HelperClassLib.Helpers.Indicators.MA(datasWithPriorDataAsDateTimeDoublePair, out ma2BeginIndex, ma2);
                        response.MovingAvarage_1_Data.points = ma1Points.Where(d => d.date >= dtFrom && d.date <= dtTo).Select(ma => (float)ma.value).ToList();
                        response.MovingAvarage_2_Data.points = ma2Points.Where(d => d.date >= dtFrom && d.date <= dtTo).Select(ma => (float)ma.value).ToList();
                    }
                }

                if (!(instrtype == "Hungarian Equities" && dtFrom.Date == dtTo.Date)) // ha nem magyar intraday
                {
                    response.SignalData.signals.AddRange(rsiSignals);
                    response.SignalData.signals.AddRange(macdSignals);
                    response.SignalData.signals = response.SignalData.signals.OrderBy(s => s.date).ToList();
                }

                // Positions
                //response.PositionsData = Signals.CalculateProfit(response.SignalData.signals);

                // Dividends
                response.DividendData.dividends = DividendHelper.GetDividends(datas.Where(d => d.timestamp >= dtFrom && d.timestamp <= dtTo).ToList());

                // Series ranges
                response.DateTimeData.min = response.DateTimeData.points.Min();
                response.DateTimeData.max = response.DateTimeData.points.Max();

                // CHART:  NORMAL
                if (response.PriceData.ohlcpoints.Any())
                {
                    response.PriceData.min = response.PriceData.ohlcpoints.Where(d => d.low > 0).Select(d => d.low).Min();
                    response.PriceData.max = response.PriceData.ohlcpoints.Where(d => d.high > 0).Select(d => d.high).Max();
                }
                else
                {
                    response.PriceData.min = response.PriceData.points.Where(d => d > 0).Min();
                    response.PriceData.max = response.PriceData.points.Max();
                }

                // CHART:  HEIKIN ASHI    
                if (p.heikinashi && (!(instrtype == "Hungarian Equities" && dtFrom.Date == dtTo.Date)) ) // ha nem magyar intraday
                {
                    response.PriceData.ohlcpoints = HelperClassLib.Helpers.Indicators.GetHeikinAshiOhlcChart(response.PriceData.ohlcpoints);
                }

                var minmaxintv = Interval.Calculate(response.PriceData.min, response.PriceData.max);
                response.PriceData.min = minmaxintv.Item1;
                response.PriceData.max = minmaxintv.Item2;
                response.PriceData.interval = minmaxintv.Item3;

                response.PercentData.min = response.PercentData.points.Min();
                response.PercentData.max = response.PercentData.points.Max();
                minmaxintv = Interval.Calculate(response.PercentData.min, response.PercentData.max);
                response.PercentData.min = minmaxintv.Item1;
                response.PercentData.max = minmaxintv.Item2;
                response.PercentData.interval = minmaxintv.Item3;

                if (response.MovingAvarage_1_Data.points.Where(d => d > 0).Any())
                {
                    response.MovingAvarage_1_Data.min = response.PriceData.min;
                    response.MovingAvarage_1_Data.max = response.PriceData.max;
                }
                if (response.MovingAvarage_2_Data.points.Where(d => d > 0).Any())
                {
                    response.MovingAvarage_2_Data.min = response.PriceData.min;
                    response.MovingAvarage_2_Data.max = response.PriceData.max;
                }
                if (response.CapitalizationData.points.Where(d => d > 0).Any())
                {
                    response.CapitalizationData.min = response.CapitalizationData.points.Where(d => d > 0).Min();
                    response.CapitalizationData.max = response.CapitalizationData.points.Max();
                    minmaxintv = Interval.Calculate(response.CapitalizationData.min, response.CapitalizationData.max);
                    response.CapitalizationData.min = minmaxintv.Item1;
                    response.CapitalizationData.max = minmaxintv.Item2;
                    response.CapitalizationData.interval = minmaxintv.Item3;
                }
                else
                {
                    response.CapitalizationData.min = 0;
                    response.CapitalizationData.max = 100;
                    response.CapitalizationData.interval = 20;
                }

                // A legelso erteknel a kiugroan magas erteket nem kell megjeleniteni
                if (response.VolumeData.points.Count > 10 && response.VolumeData.points.Max() == response.VolumeData.points.First())
                {
                    response.VolumeData.points.RemoveAt(0);
                    response.VolumeData.points.Insert(0, 0);
                }
                // Es a legutolsonal se
                if (response.VolumeData.points.Count > 10 && response.VolumeData.points.Max() == response.VolumeData.points.Last())
                {
                    response.VolumeData.points.Remove(response.VolumeData.points.Count - 1);
                    response.VolumeData.points.Add(0);
                }
                // ez nemtudom mire kellett
                if (response.VolumeData.points.Any() && response.VolumeData.points.Min() == response.VolumeData.points.Last())
                {
                    response.VolumeData.points.RemoveAt(response.VolumeData.points.Count - 1);
                    response.VolumeData.points.Add(0);
                }
                if (response.VolumeData.points.Any())
                {
                    response.VolumeData.min = response.VolumeData.points.Min();
                    response.VolumeData.max = response.VolumeData.points.Max();
                }
                if (response.VolumeData.min == response.VolumeData.max)
                {
                    response.VolumeData.min = 0;
                    response.VolumeData.max = 100;
                    response.VolumeData.interval = 20;
                }
                if (response.MovingAvarage_1_Data.min == response.MovingAvarage_1_Data.max)
                {
                    response.MovingAvarage_1_Data.min = 0;
                    response.MovingAvarage_1_Data.max = 100;
                    response.MovingAvarage_1_Data.interval = 20;
                }
                if (response.MovingAvarage_2_Data.min == response.MovingAvarage_2_Data.max)
                {
                    response.MovingAvarage_2_Data.min = 0;
                    response.MovingAvarage_2_Data.max = 100;
                    response.MovingAvarage_2_Data.interval = 20;
                }

                //response.Ranking = Ranking.CreateRanking(datas, -1, p.instrumentName, instrumentType.Equities);

                response.TimingData.comment = "Memory consumption: " + (GC.GetTotalMemory(true) / ((1024) * (1024))).ToString() + " Mb";
                response.TimingData.miliseconds = (DateTime.Now - ReqStart).Milliseconds;
            }
            catch (Exception ex)
            {
                ApplicationUser user = null;
                try { user = await _userManager.GetUserAsync(HttpContext.User); }
                catch (Exception) { }
                Helper.LogException(ex, Request, user);
                response.error = ex.Message;
                //throw ex;
            }
            return response;
        }
    }
}