namespace DFCStats.Domain.DTOs.Tables
{
    public class TableDTO
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Guid? ClubId { get; set; }
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