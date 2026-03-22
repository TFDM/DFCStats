namespace DFCStats.Web.Models.Fixtures
{
    public class Fixtures
    {
        public Guid Id { get; set; }
        public string? Season { get; set; }
        public DateOnly Date { get; set; }
        public string? Attendance { get; set; }
        public string Competition { get; set; } = string.Empty;
        public string Outcome { get; set; } = string.Empty;
        public string Scoreline { get; set; } = string.Empty;
        public string? Teams { get; set; }
        public string? TeamsWithScore { get; set; }
        public string? Venue { get; set; }
        public bool PenaltiesRequired { get; set; }
        public string? PenaltyScoreWithOutcome { get; set; }
        public List<LineUp>? LineUp { get; set; }
    }

    public class LineUp
    {
        public Guid Id { get; set; }
        public int OrderNo { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool Started {get; set;}
        public bool Sub {get; set;}
        public bool YellowCard { get; set; }
        public bool RedCard { get; set; }
        public int Goals { get; set; }
        public Guid? ReplacedByPersonId { get; set; }
        public string? ReplacedByFirstName { get; set; }
        public string? ReplacedByLastName { get; set; }
        public int? ReplacedByTime { get; set; }
    }
}