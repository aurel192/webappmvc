using System.ComponentModel.DataAnnotations;

namespace DbConnectionClassLib.Tables
{
    public class InstrumentType
    {
        [Key]
        public int Id { get; set; }

        public int Number { get; set; }

        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
