namespace DFCStats.Domain.DTOs.Nationalities
{
    public class NationalityDTO
    {
        public Guid Id { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }

    public class NewNationalityDTO
    {
        public string Nationality { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }

    public class EditNationalityDTO
    {
        public Guid Id { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }
}