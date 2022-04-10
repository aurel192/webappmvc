using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnectionClassLib.Tables
{
    public class AV_TIME_SERIES_INTRADAY_DATA
    {
        [Key]
        public int id { get; set; }

        public DateTime timestamp { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double volume { get; set; }

        public int avtsdaId { get; set; }

        [ForeignKey("avtsdaId")]
        public AV_TIME_SERIES_DAILY_ADJUSTED AV_TimeSeriesDailyAdjusted { get; set; }

        [MinLength(1)]
        [MaxLength(10)]
        public string resoultion { get; set; } // 1min, 5min, 15min, 60min,   1d
    }
}
