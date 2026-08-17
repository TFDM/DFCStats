using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    
        public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
    }
}