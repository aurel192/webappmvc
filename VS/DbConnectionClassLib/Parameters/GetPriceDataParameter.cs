namespace DbConnectionClassLib.Parameters
{
    public class GetPriceDataParameter
    {
        public string instrumentFullPath { get; set; }
        public string instrumentName { get; set; }

        public string startDateStr { get; set; }
        public string endDateStr { get; set; }
        public string interval { get; set; }

        public int rsi { get; set; } = 0;
        public int macdfast { get; set; } = 0;
        public int macdslow { get; set; } = 0;
        public int macdsignal { get; set; } = 0;

        public int bbandperiod { get; set; } = 10;
        public int bbandup { get; set; } = 2;
        public int bbanddown { get; set; } = 2;

        public bool heikinashi { get; set; } = false;
    }
}
