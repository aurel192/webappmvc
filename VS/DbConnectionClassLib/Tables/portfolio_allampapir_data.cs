using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbConnectionClassLib.Tables
{
    public class portfolio_allampapir_data
    {
        [Key]
        public int id { get; set; }

        public int AllampapirId { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }

        [ForeignKey("AllampapirId")]
        public portfolio_allampapir Portfolio_allampapir { get; set; }
    }
}
