namespace DFCStats.Web.Models.Fixtures
{
    public class Fixtures
    {
        public Guid Id { get; set; }
        public string? Season { get; set; }
        public DateOnly Date { get; set; }
        public string? Attendance { get; set; }
        public string Competition { get; set; } = string.Empty;
        public string Outcome { get; set; } = string.Empty;
        public string Scoreline { get; set; } = string.Empty;
        public string? Teams { get; set; }
    }
}