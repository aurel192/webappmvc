using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbConnectionClassLib.Tables
{
    public class AV_OVERVIEW
    {
        [Key]
        public int id { get; set; }

        public DateTime timestamp { get; set; }

        public string data { get; set; }

        public int avtsdaId { get; set; }
    }
}
