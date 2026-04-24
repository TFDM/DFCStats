using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class View_Seasons
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Loses { get; set; }
        public decimal? WinPercentage { get; set; }
        public int? AverageHomeAttendance { get; set; }
        public int? HighestHomeAttendance { get; set; }
        public int PlayersUsed { get; set; }
    }
}