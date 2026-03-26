namespace DFCStats.Domain.DTOs.Fixtures
{
    public class ParticipationFixtureDTO
    {
        public Guid ParticipationId { get; set; }
        public Guid FixtureId { get; set; }
        public DateOnly Date { get; set; }
        public string? TeamsWithScore { get; set; }
        public string? Competition { get; set; }
        public string? Scoreline { get; set; }
        public string? Outcome { get; set; }
        public string? Season { get; set; }
        public int Goals { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}

