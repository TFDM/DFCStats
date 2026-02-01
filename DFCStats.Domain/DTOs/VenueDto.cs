namespace DFCStats.Domain.DTOs
{
    public class VenueDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
}