namespace DFCStats.Web.Models.People
{
    public class Person
    {
        public Guid Id { get; set; }
        public DateOnly? DateOfBirth { get; set; } 
        public string? Nationality { get; set; }
        public string? NationalityIcon { get; set; }
        public string? Biography { get; set; }
        public List<SeasonalAppearances>? AppearancesBySeason = null!;
        public int? TotalStarts { get; set; }
        public int? TotalSubs { get; set; }
        public int? TotalGoals { get; set; }
        public int? TotalRedCards { get; set; }
        public int? TotalLeagueStarts { get; set; }
        public int? TotalLeagueSubs { get; set; }
        public int? TotalLeagueGoals { get; set; }
        public int? TotalCupStarts { get; set; }
        public int? TotalCupSubs { get; set; }
        public int? TotalCupGoals { get; set; }
        public int? TotalPlayOffStarts { get; set; }
        public int? TotalPlayOffSubs { get; set; }
        public int? TotalPlayOffGoals { get; set; }
    }

    public class SeasonalAppearances
    {
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
    }

}