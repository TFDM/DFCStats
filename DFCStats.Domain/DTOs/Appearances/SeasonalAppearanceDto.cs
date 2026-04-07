namespace DFCStats.Domain.DTOs.Appearances
{
    public class SeasonalAppearanceDTO
    {
        public Guid? PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? SeasonId { get; set; }
        public string? SeasonDescription { get; set; }
        public int? TotalAppearances { get; set; }
        public int? Starts { get; set; }
        public int? Subs { get; set; }
        public int? Goals { get; set; }
        public int? RedCards { get; set; }
        public int? LeagueStarts { get; set; }
        public int? LeagueSubs { get; set; }
        public int? LeagueGoals { get; set; }
        public int? CupStarts { get; set; }
        public int? CupSubs { get; set; }
        public int? CupGoals { get; set; }
        public int? PlayOffStarts { get; set; }
        public int? PlayOffSubs { get; set; }
        public int? PlayOffGoals { get; set; }
        public decimal? GoalsPerGame { get; set; }
    }
}