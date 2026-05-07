namespace DFCStats.Web.Models.Managers
{
    public class Manager
    {
        public Guid Id { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        public string TimeInCharge { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Nationality { get; set; }
        public string? NationalityIcon { get; set; }
        public bool CurrentlyOnGoing { get; set; }
        public bool Caretaker { get; set; }
        public int GamesManaged { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Loses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public string? WinPercentage { get; set; }
    }
}