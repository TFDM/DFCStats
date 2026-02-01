using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Venue
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string ShortDescription { get; set; } = string.Empty;
        [Required]
        public int OrderNo { get; set; }
    }
}