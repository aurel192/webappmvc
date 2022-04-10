using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbConnectionClassLib.Tables
{
    public class portfolio_hunfund_data
    {
        [Key]
        public int id { get; set; }

        public int FundId { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }
        public double Capitalization { get; set; }
        public double Cashflow { get; set; }
        public double YearlyYield { get; set; }

        [ForeignKey("FundId")]
        public portfolio_hunfund Portfolio_hunfund { get; set; }
    }
}
