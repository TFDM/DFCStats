namespace DFCStats.Domain.DTOs.Fixtures
{
    public class FixtureDTO
    {
        
    }

    public class NewFixtureDTO
    {
        public Guid SeasonId { get; set; }
        public DateOnly Date { get; set; }
        public Guid ClubId { get; set; }
        public Guid CategoryId { get; set; }
        public string Competition { get; set; } = string.Empty;
        public Guid VenueId { get; set; }
        public int DarlingtonScore { get; set; }
        public int OppositionScore { get; set; }
        public bool PenaltiesRequired { get; set; }
        public int? DarlingtonPenaltyScore { get; set; }
        public int? OppositionPenaltyScore { get; set; }
        public int? Attendance { get; set; }
        public string? Notes { get; set; }
    }
}