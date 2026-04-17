using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Season
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Description { get; set; } = string.Empty;

        // public virtual ICollection<PersonSeason> PersonSeasons { get; set; } = new List<PersonSeason>();
        // public virtual ICollection<Fixture> Fixtures { get; set; } = new List<Fixture>();

        public virtual ICollection<PersonSeason> PersonSeasons { get; set; } = null!;
        public virtual ICollection<Fixture> Fixtures { get; set; } = null!;
    }
}