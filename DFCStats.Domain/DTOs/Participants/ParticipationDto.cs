namespace DFCStats.Domain.DTOs.Participants
{
    public class ParticipationDTO
    {
        public Guid Id { get; set; }
        public Guid FixtureId { get; set; }
        public Guid? SeasonId { get; set; }
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
        public string? ReplaceByLastName { get; set; }
        public int? ReplacedByTime { get; set; }
        public string? TeamAndScore {get; set;}
        public DateOnly? Date { get; set; }
        public string? Season { get; set; }
    }
}