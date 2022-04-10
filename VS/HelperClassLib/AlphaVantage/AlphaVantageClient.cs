using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using static HelperClassLib.Helpers.HelperClass;
using static DbConnectionClassLib.Data.DatabaseInstance;
using HelperClassLib;
using System.Linq;

namespace HelperClassLib.AlphaVantage
{
    public enum AlphaVantageDatatype { CSV, JSON }
    public enum AlphaVantageFunction { TimeSeries, Forex, Cryptocurrencies, TechnicalIndicator, SectorPerformances }
    public enum AlphaVantageTimeSeriesFunction { INTRADAY, DAILY, DAILY_ADJUSTED, WEEKLY, WEEKLY_ADJUSTED, MONTHLY, MONTHLY_ADJUSTED, GLOBAL_QUOTE, SYMBOL_SEARCH }
    public enum AlphaVantageTimeSeriesOutputSize { compact, full }
    public enum AlphaVantageTimeSeriesIntradayInterval { none, min1, min5, min15, min30, min60 }

    public class AlphaVantageClient
    {
        private object _AlphaVantageParameters;

        public AlphaVantageClient(object parameters = null)
        {
            if (parameters != null)
            {
                this.SetParameters(parameters);
            }
        }

        private void SetParameters(object parameters)
        {
            this._AlphaVantageParameters = parameters;
        }

        private string GetParameterType()
        {
            if (this._AlphaVantageParameters == null)
                return null;
            string fullName = this._AlphaVantageParameters.GetType().ToString();
            int lastIndexOfDot = fullName.LastIndexOf('.');
            if (lastIndexOfDot > 0)
            {
                return fullName.Substring(lastIndexOfDot + 1, fullName.Length - lastIndexOfDot - 1);
            }
            return fullName;
        }

        private string CallAPI()
        {
            string queryString = "";
            string av_response = "";
            string ParameterType = GetParameterType();
            switch (ParameterType)
            {
                case "AlphaVantageTimeSeriesParameters":
                    AlphaVantageTimeSeriesParameters AlphaVantageTimeSeriesParameters = this._AlphaVantageParameters as AlphaVantageTimeSeriesParameters;
                    queryString = AlphaVantageTimeSeriesParameters.GetQueryString() + "&apikey=" + Constants.AVKey;
                    av_response = Http.Get("http://www.alphavantage.co", queryString).Result;
                    CreateLogEntry(queryString, "av_response.Length: " + av_response.Length.ToString());
                    return av_response;
                case "AlphaVantageSymbolSearchParameters":
                    AlphaVantageSymbolSearchParameters AlphaVantageSymbolSearchParameters = this._AlphaVantageParameters as AlphaVantageSymbolSearchParameters;
                    queryString = AlphaVantageSymbolSearchParameters.GetQueryString() + "&apikey=" + Constants.AVKey;
                    av_response = Http.Get("http://www.alphavantage.co", queryString).Result;
                    CreateLogEntry(queryString, "av_response.Length: " + av_response.Length.ToString());
                    return av_response;
                case "AlphaVantageListingParameters":
                    AlphaVantageListingParameters AlphaVantageListingParameters = this._AlphaVantageParameters as AlphaVantageListingParameters;
                    queryString = AlphaVantageListingParameters.GetQueryString() + "&apikey=" + Constants.AVKey;
                    av_response = Http.Get("http://www.alphavantage.co", queryString).Result;
                    CreateLogEntry(queryString, "av_response.Length: " + av_response.Length.ToString());
                    return av_response;
                case "AlphaVantageOverviewParameters":
                    AlphaVantageOverviewParameters AlphaVantageOverviewParameters = this._AlphaVantageParameters as AlphaVantageOverviewParameters;
                    queryString = AlphaVantageOverviewParameters.GetQueryString() + "&apikey=" + Constants.AVKey;
                    av_response = Http.Get("http://www.alphavantage.co", queryString).Result;
                    CreateLogEntry(queryString, "av_response.Length: " + av_response.Length.ToString());
                    return av_response;
                default:
                    return "error";
            }
        }

        public string GetDataOverview()
        {
            try
            {
                string av_responseCsv = this.CallAPI();
                if (av_responseCsv.Length == 236) throw new Exception("Alpha Vantage API Call Frequency Exception");
                return av_responseCsv;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                throw ex;
            }
        }

        public List<TIME_SERIES_DAILY> GetDataTimeSeries()
        {
            try
            {
                string av_responseCsv = this.CallAPI();
                if (av_responseCsv.Length == 236) throw new Exception("Alpha Vantage API Call Frequency Exception");
                List<TIME_SERIES_DAILY> parsedData = Helper.ParseCSV<TIME_SERIES_DAILY>(av_responseCsv, ",").ToList();
                parsedData = parsedData.OrderBy(d => d.timestamp).ToList();
                return parsedData;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                throw ex;
            }
        }

        public List<AV_SYMBOL_SEARCH_RESPONSE> GetDataSymbolSearch()
        {
            try
            {
                string av_responseCsv = this.CallAPI();
                List<AV_SYMBOL_SEARCH_RESPONSE> parsedData = Helper.ParseCSV<AV_SYMBOL_SEARCH_RESPONSE>(av_responseCsv, ",").ToList();
                parsedData = parsedData.OrderBy(d => d.name).ToList();
                return parsedData;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                throw ex;
            }
        }

        public List<AV_LISTING> GetDataListingStatus()
        {
            try
            {
                string av_responseCsv = this.CallAPI();
                List<AV_LISTING> parsedData = Helper.ParseCSV<AV_LISTING>(av_responseCsv, ",").ToList();
                return parsedData;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                throw ex;
            }

        }

        private void CreateLogEntry(string Key, string Value)
        {
            try
            {
                string comment = "";
                if (Environment.StackTrace.Contains("UpdateClosePrices"))
                {
                    comment += " AV_SCHEDULED " + DateTime.Now.ToDateTimeString();
                }
                DbConnectionClassLib.Tables.DBLog logEntry = new DbConnectionClassLib.Tables.DBLog()
                {
                    TimeStamp = DateTime.UtcNow,
                    Key = Key.Replace("&apikey=7JXQ", "").SubstringToMaxLength(100),
                    Value = Value.SubstringToMaxLength(100000) + comment
                };
                Db.db_log.Add(logEntry);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
    }



    public class AlphaVantageParametersCommon
    {
        public Dictionary<string, string> dict = new Dictionary<string, string>();

        public string GetQuery()
        {
            string result = "/query?";
            foreach (var kvp in dict)
                result += kvp.Key + "=" + kvp.Value + "&";
            result = result.TrimEnd('&');
            return result;
        }
    }

    public class AlphaVantageTimeSeriesParameters : AlphaVantageParametersCommon
    {
        private string _symbol = "";
        private AlphaVantageTimeSeriesFunction _timeSeriesFunction;
        private AlphaVantageDatatype _dataType;
        private AlphaVantageTimeSeriesOutputSize _outputSize;
        private AlphaVantageTimeSeriesIntradayInterval? _interval = null;

        public AlphaVantageTimeSeriesParameters(string symbol, AlphaVantageDatatype dataType, AlphaVantageTimeSeriesFunction timeSeriesFunction, AlphaVantageTimeSeriesOutputSize outputSize, AlphaVantageTimeSeriesIntradayInterval? interval = null)
        {
            this._symbol = symbol;
            this._timeSeriesFunction = timeSeriesFunction;
            this._dataType = dataType;
            this._outputSize = outputSize;
            this._interval = interval;
        }

        public Dictionary<string, string> GetParameters()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("symbol", this._symbol);
            switch (this._dataType)
            {
                case AlphaVantageDatatype.CSV:
                    dict.Add("datatype", "csv");
                    break;
                case AlphaVantageDatatype.JSON:
                    dict.Add("datatype", "json");
                    break;
            }
            switch (this._outputSize)
            {
                case AlphaVantageTimeSeriesOutputSize.compact:
                    dict.Add("outputsize", "compact");
                    break;
                case AlphaVantageTimeSeriesOutputSize.full:
                    dict.Add("outputsize", "full");
                    break;
            }
            switch (this._timeSeriesFunction)
            {
                case AlphaVantageTimeSeriesFunction.DAILY:
                    dict.Add("function", "TIME_SERIES_DAILY");
                    break;
                case AlphaVantageTimeSeriesFunction.DAILY_ADJUSTED:
                    dict.Add("function", "TIME_SERIES_DAILY_ADJUSTED");
                    break;
                case AlphaVantageTimeSeriesFunction.WEEKLY:
                    dict.Add("function", "TIME_SERIES_WEEKLY");
                    break;
                case AlphaVantageTimeSeriesFunction.WEEKLY_ADJUSTED:
                    dict.Add("function", "TIME_SERIES_WEEKLY_ADJUSTED");
                    break;
                case AlphaVantageTimeSeriesFunction.MONTHLY:
                    dict.Add("function", "TIME_SERIES_MONTHLY");
                    break;
                case AlphaVantageTimeSeriesFunction.MONTHLY_ADJUSTED:
                    dict.Add("function", "TIME_SERIES_MONTHLY_ADJUSTED");
                    break;
                case AlphaVantageTimeSeriesFunction.INTRADAY:
                    dict.Add("function", "TIME_SERIES_INTRADAY");
                    switch (this._interval)
                    {
                        case AlphaVantageTimeSeriesIntradayInterval.min1:
                            dict.Add("interval", "1min");
                            break;
                        case AlphaVantageTimeSeriesIntradayInterval.min5:
                            dict.Add("interval", "5min");
                            break;
                        case AlphaVantageTimeSeriesIntradayInterval.min15:
                            dict.Add("interval", "15min");
                            break;
                        case AlphaVantageTimeSeriesIntradayInterval.min30:
                            dict.Add("interval", "30min");
                            break;
                        case AlphaVantageTimeSeriesIntradayInterval.min60:
                            dict.Add("interval", "60min");
                            break;
                        default:
                            throw new Exception("TimeSeriesIntradayInterval is not set");
                    }
                    break;
            }
            return dict;
        }

        public string GetQueryString()
        {
            dict = this.GetParameters();
            string query = base.GetQuery();
            return query;
        }
    }

    public class AlphaVantageSymbolSearchParameters : AlphaVantageParametersCommon
    {
        private string _keywords = "";
        private AlphaVantageDatatype _dataType;

        public AlphaVantageSymbolSearchParameters(string keywords, AlphaVantageDatatype dataType = AlphaVantageDatatype.CSV)
        {
            this._keywords = keywords;
            this._dataType = dataType;
        }

        public Dictionary<string, string> GetParameters()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("function", "SYMBOL_SEARCH");
            dict.Add("keywords", this._keywords);
            switch (this._dataType)
            {
                case AlphaVantageDatatype.CSV:
                    dict.Add("datatype", "csv");
                    break;
                case AlphaVantageDatatype.JSON:
                    dict.Add("datatype", "json");
                    break;
            }
            return dict;
        }

        public string GetQueryString()
        {
            dict = this.GetParameters();
            string query = base.GetQuery();
            return query;
        }
    }

    public class AlphaVantageOverviewParameters : AlphaVantageParametersCommon
    {
        private string _symbol = "";
        private AlphaVantageDatatype _dataType;

        public AlphaVantageOverviewParameters(string symbol)
        {
            this._symbol = symbol;
        }

        public Dictionary<string, string> GetParameters()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("function", "OVERVIEW");
            dict.Add("symbol", this._symbol);
            return dict;
        }

        public string GetQueryString()
        {
            dict = this.GetParameters();
            string query = base.GetQuery();
            return query;
        }
    }

    public class AlphaVantageListingParameters : AlphaVantageParametersCommon
    {
        //private string _date = "";
        //private AlphaVantageDatatype _dataType;

        public AlphaVantageListingParameters()
        {
            //this._date = date;
            //this._dataType = AlphaVantageDatatype.CSV;
        }

        public Dictionary<string, string> GetParameters()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("function", "LISTING_STATUS");
            //dict.Add("keywords", this._date);
            //switch (this._dataType)
            //{
            //    case AlphaVantageDatatype.CSV:
            //        dict.Add("datatype", "csv");
            //        break;
            //    case AlphaVantageDatatype.JSON:
            //        dict.Add("datatype", "json");
            //        break;
            //}
            return dict;
        }

        public string GetQueryString()
        {
            dict = this.GetParameters();
            string query = base.GetQuery();
            return query;
        }
    }
}
