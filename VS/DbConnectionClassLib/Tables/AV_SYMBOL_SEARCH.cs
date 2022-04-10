using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DbConnectionClassLib.Tables
{
    public class AV_SYMBOL
    {
        [Key]
        public int id { get; set; }

        [MaxLength(20)]
        public string symbol { get; set; }

        [MaxLength(100)]
        public string name { get; set; }

        [MaxLength(100)]
        public string type { get; set; }

        [MaxLength(100)]
        public string region { get; set; }

        [MaxLength(10)]
        public string marketOpen { get; set; }

        [MaxLength(10)]
        public string marketClose { get; set; }

        [MaxLength(50)]
        public string timezone { get; set; }

        [MaxLength(50)]
        public string currency { get; set; }
    }
}
