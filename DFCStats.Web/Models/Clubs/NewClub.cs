namespace DFCStats.Web.Models.Clubs
{
    public class NewClub
    {
        // Adding string? ensures that fluent validation messages for NotEmpty are shown
        public string? Name { get; set; }
    }
}