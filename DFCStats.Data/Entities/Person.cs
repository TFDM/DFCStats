using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public Guid? NationalityId { get; set; }
        public string? Biography { get; set; }
        [Required]
        public bool IsManager { get; set; }

        public List<PersonSeason> PersonSeasons { get; set; } = new List<PersonSeason>();
        public virtual Nationality? Nationality { get; set; }
        public virtual ICollection<Participation> Participation { get; set; } = new List<Participation>();
    }
}