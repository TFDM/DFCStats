using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class EmailTemplate
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Template { get; set; } = string.Empty;
        [Required]
        public bool IsHtml { get; set;}
    }
}