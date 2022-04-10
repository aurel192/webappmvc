namespace DbConnectionClassLib.ResponseClasses
{
    //symbol,name,type,region,marketOpen,marketClose,timezone,currency,matchScore
    public class AV_SYMBOL_SEARCH_RESPONSE
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string region { get; set; }
        public string marketOpen { get; set; }
        public string marketClose { get; set; }
        public string timezone { get; set; }
        public string currency { get; set; }
        public float matchScore { get; set; }

        public override string ToString()
        {
            string str = "";
            str += "\nSymbol: " + symbol;
            str += "\nName: " + name;
            str += "\nType: " + type;
            str += "\nRegion: " + region;
            str += "\nMarketOpen: " + marketOpen;
            str += "\nMarketClose: " + marketClose;
            str += "\nTimezone: " + timezone;
            str += "\nCurrency: " + currency;
            str += "\nMatchScore: " + matchScore;
            str += "\n";
            return str;
        }
    }

    public class AV_LISTING
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string exchange { get; set; }
        public string assetType { get; set; }
        public string ipoDate { get; set; }
        public string delistingDate { get; set; }
        public string status { get; set; }

        public override string ToString()
        {
            string str = "";
            str += "\nSymbol: " + symbol;
            str += "\nName: " + name;
            str += "\nExchange: " + exchange;
            str += "\nAssetType: " + assetType;
            str += "\nIPO Date: " + ipoDate;
            str += "\nDelistingDate: " + delistingDate;
            str += "\nStatus: " + status;
            str += "\n";
            return str;
        }
    }
}
