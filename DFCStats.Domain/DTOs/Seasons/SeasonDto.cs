namespace DFCStats.Domain.DTOs.Seasons
{
    public class SeasonDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set;} = string.Empty;
        public List<PersonAttachedToSeason>? PeopleAttachedToSeason { get; set; }
        public List<Fixture>? Fixtures { get; set; }
    }

    public class PersonAttachedToSeason
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastNameFirstName { get; set; } = string.Empty;
    }

    public class Fixture
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public string? Season { get; set; }
        public DateOnly Date { get; set; }
        public Guid CategoryId { get; set; }
        public string? Category { get; set; }
        public string Competition { get; set; } = string.Empty;
        public Guid ClubId { get; set; }
        public string? Club { get; set; }
        public Guid VenueId { get; set; }
        public string? Venue { get; set; }
        public string? VenueShort { get; set; }
        public string Scoreline { get; set; } = string.Empty;
        public string? TeamsAndScores {get; set; }
        public string? Teams { get; set; }
        public bool PenaltiesRequired { get; set; }
        public string? PenaltyScoreline { get; set; }
        public string? PenaltyScoreWithOutcome { get; set; }
        public string Outcome { get; set; } = string.Empty;
        public int? Attendance { get; set; }
        public int DarlingtonScore { get; set; }
        public int OppositionScore { get; set; }
        public int? DarlingtonPenaltyScore { get; set; }
        public int? OppositionPenaltyScore { get; set; }
        public string? Notes { get; set; }
    }
}