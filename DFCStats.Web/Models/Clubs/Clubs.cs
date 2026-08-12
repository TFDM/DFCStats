namespace DFCStats.Web.Models.Clubs
{
    public class Clubs
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
    }
}