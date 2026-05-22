namespace DFCStats.Web.Models.Seasons
{
    public class Seasons
    {
        public Guid Id { get; set; }
        public string Season { get; set; } = string.Empty;
        public int? GamesPlayed { get; set; }
        public int? Wins { get; set; }
        public int? Draws { get; set; }
        public int? Loses { get; set; }
        public string? WinPercentage { get; set; }
        public int? TotalPlayersUsed { get; set; }
        public string? AverageHomeAttendance { get; set; }
        public string? HighestHomeAttendance { get; set; }
        public List<SeasonFixtures>? Fixtures = null!;
        public List<SeasonalAppearances>? Appearances = null!;
        public List<SeasonalTable>? Table = null!;
    }

    public class SeasonFixtures
    {
        public Guid Id { get; set; }
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
    }

    public class SeasonalAppearances
    {
        public Guid? PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
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

    public class SeasonalTable
    {
        public Guid Id { get; set; }
        public string ClubName { get; set; } = string.Empty;
        public int Position { get; set; }
        public int GamesPlayed { get; set; }
        public int HomeGamesWon { get; set; }
        public int HomeGamesDrawn { get; set; }
        public int HomeGamesLost { get; set; }
        public int HomeGoalsFor { get; set; }
        public int HomeGoalsAgainst { get; set; }
        public int AwayGamesWon { get; set; }
        public int AwayGamesDrawn { get; set; }
        public int AwayGamesLost { get; set; }
        public int AwayGoalsFor { get; set; }
        public int AwayGoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
        public bool IsChampion { get; set; }
        public bool IsPromotion { get; set; }
        public bool IsRelegation { get; set; }
        public bool IsPlayOff { get; set; }
        public string? Notes { get; set; }
    }
}

