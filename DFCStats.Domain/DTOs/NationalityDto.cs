namespace DFCStats.Domain.DTOs
{
    public class NationalityDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public string? IconImage { get; set; }
    }
}