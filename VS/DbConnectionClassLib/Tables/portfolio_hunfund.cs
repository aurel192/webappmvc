using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbConnectionClassLib.Tables
{
    public class portfolio_hunfund
    {
        [Key]
        public int id { get; set; }

        [MinLength(1)]
        [MaxLength(200)]
        public string name { get; set; }

        public int ticker { get; set; }

        public int state { get; set; }

        public List<portfolio_hunfund_data> Data { get; set; }

        public DateTime? lastupdate { get; set; }
    }
}
