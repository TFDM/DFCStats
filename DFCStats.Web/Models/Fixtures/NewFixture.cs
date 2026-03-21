using System.ComponentModel;

namespace DFCStats.Web.Models.Fixtures
{
    public class NewFixture
    {
        [DisplayName("Season")]
        public Guid SeasonId { get; set; }
        public DateOnly Date { get; set; }
        [DisplayName("Club")]
        public Guid ClubId { get; set; }
        [DisplayName("Category")]
        public Guid CategoryId { get; set; }
        public string Competition { get; set; } = string.Empty;
        [DisplayName("Venue")]
        public Guid VenueId { get; set; }
        [DisplayName("Darlington Score")]
        public int DarlingtonScore { get; set; }
        [DisplayName("Opposition Score")]
        public int OppositionScore { get; set; }
        [DisplayName("Penalties Required")]
        public bool PenaltiesRequired { get; set; }
        [DisplayName("Darlington Penalties")]
        public int? DarlingtonPenaltyScore { get; set; }
        [DisplayName("Opposition Penalties")]
        public int? OppositionPenaltyScore { get; set; }
        public int? Attendance { get; set; }
        public string? Notes { get; set; }
    }
}