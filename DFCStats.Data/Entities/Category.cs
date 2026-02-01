using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public bool League { get; set; }
        [Required]
        public bool Cup { get; set; }
        [Required]
        public bool FootballLeague { get; set; }
        [Required]
        public bool NonLeague { get; set; }
        [Required]
        public bool PlayOff { get; set; }
        [Required]
        public int OrderNo { get; set; }
    }
}



