using System.ComponentModel.DataAnnotations;

namespace DFCStats.Web.Models.Nationalities
{
    public class NewNationality
    {
        public string Nationality { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }
}