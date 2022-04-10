using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbConnectionClassLib.Tables
{
    public class portfolio_hunstock
    {
        [Key]
        public int id { get; set; }

        [MinLength(1)]
        [MaxLength(50)]
        public string name { get; set; }

        public int ticker { get; set; }

        public int state { get; set; }

        public List<portfolio_hunstock_data> Data { get; set; }

        public DateTime? lastupdate { get; set; }
    }
}
