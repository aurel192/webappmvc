using DbConnectionClassLib.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbConnectionClassLib.ResponseClasses
{
    public class TSDAListItem
    {
        public List<TIME_SERIES_DAILY_ADJUSTED_MIN> datas { get; set; }
        public string name { get; set; }
    }

    public class TSDACListItem
    {
        public List<TIME_SERIES_DAILY_ADJUSTED> datas { get; set; }
        public string name { get; set; }
    }

    public class CompareResponse
    {
        public DateTimeData DateTimeData { get; set; }
        public List<PercentData> PercentData { get; set; }
        public List<RankingItem> RankingData { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public string comment { get; set; } = "";
        public string error { get; set; }
        public CompareResponse()
        {
            this.DateTimeData = new DateTimeData();
            this.PercentData = new List<PercentData>();
            this.RankingData = new List<RankingItem>();
        }
    }

    public class ChartResponse
    {
        public DateTimeData DateTimeData { get; set; }
        public TimeData TimeData { get; set; }

        public PriceData PriceData { get; set; }
        public PercentData PercentData { get; set; }
        public CapitalizationData CapitalizationData { get; set; }
        public VolumeData VolumeData { get; set; }

        public MovingAvarageData MovingAvarage_1_Data { get; set; }
        public MovingAvarageData MovingAvarage_2_Data { get; set; }

        public RsiData RsiData { get; set; }
        public MacdData MacdData { get; set; }
        public BbandsData BbandsData { get; set; }

        public SignalData SignalData { get; set; }
        public DividendData DividendData { get; set; }
        public PositionsData PositionsData { get; set; }

        public TimingData TimingData { get; set; }

        public RankingItem Ranking { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }
        public string type { get; set; } = "daily";

        //public TimeSpan From { get; set; }
        //public TimeSpan To { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public string error { get; set; }
        public string comment { get; set; } = "";

        public string overview { get; set; } = "";

        public ChartResponse(string Name, string ShortName, TimeSpan From, TimeSpan To, bool InitPriceData = true, bool InitVolumeData = true, bool InitRsiData = true, bool InitMacdData = true, bool InitSignalData = true, bool InitPositionsData = true, bool InitBbandsData = true, bool InitMovingAvarageData = true, bool InitDividendData = true, string type = "intraday")
        {
            this.Name = Name;
            this.ShortName = ShortName;
            this.error = null;
            //this.From = From;
            //this.To = To;
            this.type = type;
            this.TimeData = new TimeData();
            InitData(InitPriceData, InitVolumeData, InitRsiData, InitMacdData, InitSignalData, InitPositionsData, InitBbandsData, InitMovingAvarageData, InitDividendData);
        }

        public ChartResponse(string Name, string ShortName, DateTime DateFrom, DateTime DateTo, bool InitPriceData = true, bool InitVolumeData = true, bool InitRsiData = true, bool InitMacdData = true, bool InitSignalData = true, bool InitPositionsData = true, bool InitBbandsData = true, bool InitMovingAvarageData = true, bool InitDividendData = true, string type = "daily")
        {
            this.Name = Name;
            this.ShortName = ShortName;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.type = type;
            this.DateTimeData = new DateTimeData();
            InitData(InitPriceData, InitVolumeData, InitRsiData, InitMacdData, InitSignalData, InitPositionsData, InitBbandsData, InitMovingAvarageData, InitDividendData);
        }

        public void InitData(bool InitPriceData, bool InitVolumeData, bool InitRsiData, bool InitMacdData, bool InitSignalData, bool InitPositionsData, bool InitBbandsData, bool InitMovingAvarageData, bool InitDividendData)
        {
            if (InitPriceData)
            {
                this.PriceData = new PriceData("Price", "line");
                this.PercentData = new PercentData("Percent", "line");
            }
            if (InitVolumeData)
            {
                this.CapitalizationData = new CapitalizationData("Capitalization", "line");
                this.VolumeData = new VolumeData("Volume", "line");
            }
            if (InitMovingAvarageData)
            {
                this.MovingAvarage_1_Data = new MovingAvarageData("MA 50", "line");
                this.MovingAvarage_2_Data = new MovingAvarageData("MA 200", "line");
            }
            if (InitRsiData)
                this.RsiData = new RsiData("RSI", "line");
            if (InitMacdData)
                this.MacdData = new MacdData();
            if (InitBbandsData)
                this.BbandsData = new BbandsData();
            if (InitSignalData)
                this.SignalData = new SignalData();
            if (InitDividendData)
                this.DividendData = new DividendData();
            if (InitPositionsData)
                this.PositionsData = new PositionsData();
            this.TimingData = new TimingData();
        }
    }

    public class TimeData
    {
        public List<TimeSpan> points { get; set; }
        public TimeSpan min { get; set; }
        public TimeSpan max { get; set; }
        public TimeData()
        {
            this.points = new List<TimeSpan>();
        }
    }

    public class DateTimeData
    {
        public List<DateTime> points { get; set; }
        public DateTime min { get; set; }
        public DateTime max { get; set; }
        public DateTimeData()
        {
            this.points = new List<DateTime>();
        }
    }

    public class PriceData : SeriesData
    {
        public List<double> points { get; set; }
        public List<double> change { get; set; }
        public List<double> changebeforeopen { get; set; }
        public List<OHLC> ohlcpoints { get; set; }
        public PriceData(string name, string type)
        {
            this.points = new List<double>();
            this.change = new List<double>();
            this.changebeforeopen = new List<double>();
            this.ohlcpoints = new List<OHLC>();
            base.name = name;
            base.type = type;
        }
    }

    public class OHLC
    {
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
    }

    public class PercentData : SeriesData
    {
        public List<double> points { get; set; }
        public string color { get; set; }
        public PercentData(string name, string type, string color = "#0000FF")
        {
            this.points = new List<double>();
            base.name = name;
            base.type = type;
            this.color = color;
        }
    }

    public class TimingData
    {
        public List<Timing> times { get; set; }
        public int miliseconds { get; set; }
        public string comment { get; set; }
        public TimingData()
        {
            this.times = new List<Timing>();
        }
    }

    public class CapitalizationData : SeriesData
    {
        public List<double> points { get; set; }
        public CapitalizationData(string name, string type)
        {
            this.points = new List<double>();
            base.name = name;
            base.type = type;
        }
    }

    public class VolumeData : SeriesData
    {
        public List<float> points { get; set; }
        public VolumeData(string name, string type)
        {
            this.points = new List<float>();
            base.name = name;
            base.type = type;
        }
    }

    public class MovingAvarageData : SeriesData
    {
        public List<float> points { get; set; }
        public MovingAvarageData(string name, string type)
        {
            this.points = new List<float>();
            base.name = name;
            base.type = type;
        }
    }

    public class RsiData : SeriesData
    {
        public List<float> points { get; set; }
        public RsiData(string name, string type)
        {
            this.points = new List<float>();
            base.name = name;
            base.type = type;
        }
    }

    public class MacdData
    {
        public Macd Macd { get; set; }
        public Macd MacdHistogram { get; set; }
        public Macd MacdSignal { get; set; }
        public MacdData()
        {
            this.Macd = new Macd("MACD", "line");
            this.MacdHistogram = new Macd("MACD Histogram", "line");
            this.MacdSignal = new Macd("MACD Signal", "line");
        }
    }

    public class Macd : SeriesData
    {
        public List<float> points { get; set; }
        public Macd(string name, string type)
        {
            this.points = new List<float>();
            base.name = name;
            base.type = type;
        }
    }

    public class BbandsData
    {
        public BBand upper { get; set; }
        public BBand middle { get; set; }
        public BBand lower { get; set; }
        public BbandsData()
        {
            this.upper = new BBand("Bband Upper", "line");
            this.middle = new BBand("Bband Middle", "line");
            this.lower = new BBand("Bband Lower", "line");
        }
    }

    public class BBand : SeriesData
    {
        public List<float> points { get; set; }
        public BBand(string name, string type)
        {
            this.points = new List<float>();
            base.name = name;
            base.type = type;
        }
    }

    public class SeriesData
    {
        public SeriesData()
        {
        }
        public SeriesData(string name, string type)
        {
            this.name = name;
            this.type = type;
        }
        public string name { get; set; } = "Name";
        public string type { get; set; } = "line";
        public double min { get; set; }
        public double max { get; set; }
        public double interval { get; set; }
    }

    public class MACDValues : DateTimeDoublePair
    {
        public float macd { get; set; }
        public float hist { get; set; }
        public float signal { get; set; }
    }

    public class BBandValues : DateTimeDoublePair
    {
        public float upper { get; set; }
        public float middle { get; set; }
        public float lower { get; set; }
    }

    public class DateTimeDoublePair
    {
        public DateTime date { get; set; }
        public double value { get; set; }
    }

    public class SignalData
    {
        public SignalData()
        {
            this.signals = new List<Signal>();
        }
        public List<Signal> signals { get; set; }
    }

    public class DividendData
    {
        public DividendData()
        {
            this.dividends = new List<Dividend>();
        }
        public List<Dividend> dividends { get; set; }
    }

    /********************************************/

    public class StockDataResponse
    {
        public List<SeriesObject> list { get; set; }
        public double minPrice { get; set; }
        public double maxPrice { get; set; }
        public double intvPrice { get; set; }
        public double minVolume { get; set; }
        public double maxVolume { get; set; }
        public double intvVolume { get; set; }
        public macdminmax MacdValues { get; set; }
        public List<Signal> signals { get; set; }
        public PositionsData positions { get; set; }
        public List<Dividend> dividends { get; set; }
        public List<Timing> timings { get; set; }
        public string comment { get; set; }
        public List<Point> VolumeData { get; set; }

        public StockDataResponse()
        {
            this.VolumeData = new List<Point>();
            this.timings = new List<Timing>();
            this.dividends = new List<Dividend>();
            this.list = new List<SeriesObject>();
            this.MacdValues = new macdminmax();
            this.signals = new List<Signal>();
            this.positions = new PositionsData();
        }
    }

    //public class SeriesObject {}

    public class Dividend
    {
        public int pos { get; set; } // x tengely mentén hanyadik ponthoz tartozik
        public double value { get; set; } // y tengely (ár)
        public string comment { get; set; }
        public string date { get; set; }
        public double dividendValue { get; set; } 
    }

    public class Timing
    {
        public TimeSpan time { get; set; }
        public string function { get; set; }
    }

    public class Signal : Point
    {
        public int pos { get; set; } // x tengely mentén hanyadik ponthoz tartozik
        public string type { get; set; } // BUY - SELL,  LONG, SHORT, CLOSE
        public string comment { get; set; }
        public double signalValue { get; set; } // Pl RSI 29 v MACD -21.5
    }

    public class macdminmax
    {
        public double macdMin { get; set; }
        public double macdMax { get; set; }
        public double macdHistMin { get; set; }
        public double macdHistMax { get; set; }
        public double macdSignalMin { get; set; }
        public double macdSignalMax { get; set; }

        public double macd { get; set; }
        public double hist { get; set; }
        public double sig { get; set; }
    }

    public class PointBase
    {
        public string x { get; set; } // Dátum
        public DateTime date { get; set; }
    }

    public class Point : PointBase
    {
        public double value { get; set; }
    }

    public class RSIPoint : Point
    {
        public float rsi { get; set; }
    }

    public class MACDPoint : Point
    {
        public double macd { get; set; }
        public double hist { get; set; }
        public double signal { get; set; }
    }

    public class SeriesObject
    {
        public List<Point> points { get; set; }
        public string name { get; set; } = "Name";
        public string type { get; set; } = "line";
    }


    public class PositionsData
    {
        public List<OpenPosition> openPositions { get; set; }
        public List<ClosedPosition> closedPositions { get; set; }
        public double sumProfit { get; set; }

        public PositionsData()
        {
            this.openPositions = new List<OpenPosition>();
            this.closedPositions = new List<ClosedPosition>();
        }
    }

    public class OpenPosition
    {
        //public string openedStr { get; set; } // Depricated
        public double openPrice { get; set; }
        public DateTime opened { get; set; }
    }

    public class ClosedPosition : OpenPosition
    {
        //public string closedStr { get; set; } // Depricated
        public double closePrice { get; set; }
        public double profit { get; set; }
        public double profitPercent { get; set; }
        public DateTime closed { get; set; }
        public int interval { get; set; }
    }

    public class PORFOLIO_HUN_EQUITY
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Avg { get; set; }
        public double Volume { get; set; }
        public double VolumeCount { get; set; }
    }

    public class PORFOLIO_HUN_MUTUALFUND
    {
        public DateTime Date { get; set; }
        public double Capitalization { get; set; } // Nettó eszközérték
        public double Cashflow { get; set; }
        public double Price { get; set; } // Egy jegyre jutó nettó eszközérték
        public double YearlyYield { get; set; }
    }


    public class PORFOLIO_HUN_ALLAMPAPIR
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }

    public class TIME_SERIES_DAILY_ADJUSTED
    {
        public DateTime timestamp { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double adjusted_close { get; set; }
        public double volume { get; set; }
        public double dividend_amount { get; set; }
        public double split_coefficient { get; set; }
        public double capitalization { get; set; }
    }

    public class TIME_SERIES_DAILY
    {
        public DateTime timestamp { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double volume { get; set; }
    }

    public class TIME_SERIES_DAILY_ADJUSTED_MIN
    {
        public DateTime timestamp { get; set; }
        public double adjusted_close { get; set; }
        public double volume { get; set; }
        public double dividend_amount { get; set; }
    }
    
    public class RankingItem
    {
        public int alapid { get; set; }
        public string name { get; set; }
        public instrumentType type { get; set; }
        public double capitalization { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public double yieldPercent { get; set; }
        public int days { get; set; }
        public List<YieldInYear> yearlyYields { get; set; }
        public List<YieldInMonth> monthlyYields { get; set; }
        public double avgYearlyYield { get; set; }
        public double yearlyYield { get; set; }
        public double maxDropPercent { get; set; }
        public int maxDaysWithoutRise { get; set; }
        public List<Drop> DropList { get; set; }
        public List<DaysWithoutRise> DaysWithoutRise { get; set; }
        public RankingItem()
        {
            this.yearlyYields = new List<YieldInYear>();
            this.monthlyYields = new List<YieldInMonth>();
            this.DropList = new List<Drop>();
            this.DaysWithoutRise = new List<DaysWithoutRise>();
        }
    }

    public class Drop
    {
        public double percent { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public DateTime? Recovered { get; set; }
        public int? RecoveryDays { get; set; }
    }

    public class DaysWithoutRise
    {
        public double dropPercent { get; set; }
        public int days { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }

    public class YieldInYear
    {
        public int year { get; set; }
        public double percent { get; set; }
    }

    public class YieldInMonth
    {
        public DateTime month { get; set; }
        public double percent { get; set; }
        public YieldInMonth(DateTime month)
        {
            this.month = month;
        }
    }

    public class CompareInstrumentsParam
    {
        public List<FormElement> formData { get; set; }
        public string startDateStr { get; set; }
        public string endDateStr { get; set; }
        public DateTime datefrom { get; set; }
        public DateTime dateto { get; set; }
    }

    public class FormElement
    {
        public string text { get; set; }
        public int type { get; set; }
        public int stock { get; set; }
        public string instrumentFullPath { get; set; }
        public string instrumentName { get; set; }
        public string symbol { get; set; }
    }

    public struct IntFloatPair
    {
        public int index { get; set; }
        public float value { get; set; }
    }
}
