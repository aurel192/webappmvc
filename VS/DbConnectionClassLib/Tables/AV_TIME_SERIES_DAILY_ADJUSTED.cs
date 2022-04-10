using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnectionClassLib.Tables
{
    public class AV_TIME_SERIES_DAILY_ADJUSTED
    {
        [Key]
        public int id { get; set; }

        [MinLength(1)]
        [MaxLength(200)]
        public string name { get; set; }

        [MinLength(1)]
        [MaxLength(200)]
        public string symbol { get; set; }

        public int state { get; set; }

        public List<AV_TIME_SERIES_DAILY_ADJUSTED_DATA> Data { get; set; }

        public DateTime? lastupdate { get; set; }
    }

    public class AV_TIME_SERIES_DAILY_ADJUSTED_DATA
    {
        [Key]
        public int id { get; set; }
        
        public DateTime timestamp { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double adjusted_close { get; set; }
        public double volume { get; set; }
        public double dividend_amount { get; set; }
        public double split_coefficient { get; set; }

        public int avtsdaId { get; set; }

        [ForeignKey("avtsdaId")]
        public AV_TIME_SERIES_DAILY_ADJUSTED AV_TimeSeriesDailyAdjusted { get; set; }
    }
}
