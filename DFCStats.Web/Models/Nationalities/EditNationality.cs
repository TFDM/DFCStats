namespace DFCStats.Web.Models.Nationalities
{
    public class EditNationality : INationality
    {
        public Guid Id { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }
}