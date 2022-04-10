using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnectionClassLib.ResponseClasses
{
    public class BetCsvLine
    {
        // NAME,DATE,CLOSE,FORGDB,FORGHUF,FORGEUR,KOTSZAM,OPEN,LOW,HIGH,DEVIZA,AVGPRICE,CAPITALIZATION
        public string NAME { get; set; }
        public DateTime DATE { get; set; }
        public double CLOSE { get; set; }
        public double FORGDB { get; set; }
        public double FORGHUF { get; set; }
        public double FORGEUR { get; set; }
        public double KOTSZAM { get; set; }
        public double OPEN { get; set; }
        public double LOW { get; set; }
        public double HIGH { get; set; }
        public string DEVIZA { get; set; }
        public double AVGPRICE { get; set; }
        public string CAPITALIZATION { get; set; }
    }
}
