namespace DFCStats.Web.Models.Seasons
{
    public class NewSeason
    {
        // Adding string? ensures that fluent validation messages for NotEmpty are shown
        public string? Description { get; set; }
    }
}