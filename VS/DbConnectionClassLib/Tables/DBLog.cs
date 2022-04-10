using DbConnectionClassLib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DbConnectionClassLib.Tables
{
    public class DBLog
    {
        [Key]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        [MinLength(1)]
        [MaxLength(100)]
        public string Key { get; set; }

        [MinLength(1)]
        [MaxLength(100000)]
        public string Value { get; set; }
    }
}
