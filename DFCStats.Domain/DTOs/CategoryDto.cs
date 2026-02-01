namespace DFCStats.Domain.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool League { get; set; }
        public bool Cup { get; set; }
        public bool FootballLeague { get; set; }
        public bool NonLeague { get; set; }
        public bool PlayOff { get; set; }
        public int OrderNo { get; set; }
    }
}