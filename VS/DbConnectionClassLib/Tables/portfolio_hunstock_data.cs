using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbConnectionClassLib.Tables
{
    public class portfolio_hunstock_data
    {
        [Key]
        public int id { get; set; }

        public int StockId { get; set; }

        public DateTime Date { get; set; }

        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Avg { get; set; }
        public double Volume { get; set; }

        public int VolumeCount { get; set; }

        [ForeignKey("StockId")]
        public portfolio_hunstock Portfolio_hunstock { get; set; }
    }
}
