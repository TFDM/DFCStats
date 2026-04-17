using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFCStats.Data.Entities
{
    public class Nationality
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Column("Nationality")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;
        [MaxLength(10)]
        public string? Icon { get; set; }

        public virtual ICollection<Person> People { get; set; } = null!;
        // public virtual ICollection<Person> People { get; set; } = new List<Person>();
    }
}