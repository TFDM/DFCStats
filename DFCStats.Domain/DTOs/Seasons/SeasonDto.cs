using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.DTOs.People;
using DFCStats.Domain.DTOs.Appearances;

namespace DFCStats.Domain.DTOs.Seasons
{
    public class SeasonDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set;} = string.Empty;
        public List<PersonDTO>? PeopleAttachedToSeason { get; set; }
        public List<FixtureDTO>? Fixtures { get; set; }
        public List<SeasonalAppearanceDTO>? Appearances { get; set; }
        public int? GamesPlayed { get; set; }
        public int? GamesWon { get; set; }
        public int? GamesDrawn { get; set; }
        public int? GamesLost { get; set; }
        public decimal? WinPercentage { get; set; }
        public int? TotalPlayersUed { get; set; }
        public int? AverageHomeAttendance { get; set; }
        public int? HighestHomeAttendance { get; set; }
    }
}