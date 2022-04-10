using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnectionClassLib.Tables
{
    public class AV_DIGITAL_CURRENCY_DAILY
    {
        [Key]
        public int id { get; set; }

        [MinLength(1)]
        [MaxLength(200)]
        public string name { get; set; }

        [MinLength(1)]
        [MaxLength(200)]
        public string market { get; set; }

        [MinLength(1)]
        [MaxLength(200)]
        public string symbol { get; set; }

        public int state { get; set; }

        public List<AV_DIGITAL_CURRENCY_DAILY_DATA> Data { get; set; }

        public DateTime? lastupdate { get; set; }
    }

    public class AV_DIGITAL_CURRENCY_DAILY_DATA
    {
        [Key]
        public int id { get; set; }

        public DateTime timestamp { get; set; }
        public double openCNY { get; set; }
        public double highCNY { get; set; }
        public double lowCNY { get; set; }
        public double closeCNY { get; set; }

        public double openUSD { get; set; }
        public double highUSD { get; set; }
        public double lowUSD { get; set; }
        public double closeUSD { get; set; }

        public double volume { get; set; }
        public double marketcapUSD { get; set; }


        public int avdcdId { get; set; }

        [ForeignKey("avdcdId")]
        public AV_DIGITAL_CURRENCY_DAILY AV_DigitalCurrencyDaily { get; set; }
    }
}
