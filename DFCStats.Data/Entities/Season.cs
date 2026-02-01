using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Season
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Description { get; set; } = string.Empty;
    }
}