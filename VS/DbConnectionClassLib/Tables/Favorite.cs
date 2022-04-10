using DbConnectionClassLib.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbConnectionClassLib.Tables
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }


        [MinLength(1)]
        [MaxLength(200)]
        public string Type { get; set; }

        [MinLength(1)]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
