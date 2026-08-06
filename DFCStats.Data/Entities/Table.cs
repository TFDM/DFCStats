using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Table
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Guid? ClubId { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int HomeWon { get; set; }
        public int HomeDrawn { get; set; }
        public int HomeLost { get; set; }
        public int HomeGoalsFor { get; set; }
        public int HomeGoalsAgainst { get; set; }
        public int AwayWon { get; set; }
        public int AwayDrawn { get; set; }
        public int AwayLost { get; set; }
        public int AwayGoalsFor { get; set; }
        public int AwayGoalsAgainst { get; set; }
        public int Points { get; set; }
        public bool IsChampion { get; set; }
        public bool IsPromotion { get; set; }
        public bool IsPlayOffs { get; set; }
        public bool IsRelegated { get; set; }
        public bool IsDarlington { get; set; }
        public string? Notes { get; set; }

        public virtual Season? Season { get; set; }
        public virtual Club? Club { get; set; }
    }
}