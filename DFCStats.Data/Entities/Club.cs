using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Club
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}