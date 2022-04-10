using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbConnectionClassLib.Tables
{
    public class portfolio_allampapir
    {
        [Key]
        public int id { get; set; }

        [MinLength(1)]
        [MaxLength(200)]
        public string name { get; set; }

        public string code { get; set; }
        
        public int state { get; set; }

        public List<portfolio_allampapir_data> Data { get; set; }

        public DateTime? lastupdate { get; set; }
    }
}
