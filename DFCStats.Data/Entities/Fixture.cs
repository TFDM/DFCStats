using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFCStats.Data.Entities
{
    public class Fixture
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid SeasonId { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public Guid ClubId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Competition { get; set; } = string.Empty;
        [Required]
        public Guid VenueId { get; set; }
        [Required]
        public Int32 DarlingtonScore { get; set; }
        [Required]
        public Int32 OppositionScore { get; set; }
        public int? DarlingtonPenaltyScore { get; set; }
        public int? OppositionPenaltyScore { get; set; }
        public int? Attendance { get; set; }
        [Required]
        [MaxLength(1)]
        public string Outcome { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public virtual Season? Season { get; set; }
        public virtual Club? Club { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Venue? Venue { get; set; }

        // public virtual ICollection<Participation> Participants { get; set; } = new List<Participation>();

        public virtual ICollection<Participation> Participants { get; set; } = null!;
    }
}