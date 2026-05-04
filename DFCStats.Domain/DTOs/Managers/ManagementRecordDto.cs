namespace DFCStats.Domain.DTOs.Managers
{
    public class ManagementRecordDTO
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int DaysInCharge { get; set; }
        public bool IsCaretaker { get; set; }
        public int NumberOfGamesManaged { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public decimal? WinPercentage { get; set; }

    }
}