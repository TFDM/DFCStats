using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Manager
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PersonId { get; set; }
        [Required]
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        [Required]
        public bool IsCaretaker { get; set; }
        public virtual Person? Person { get; set; }
    }
}