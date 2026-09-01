using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string EmailAddress { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Salt { get; set; } = string.Empty;
        [Required]
        public bool AllowLogin { get; set; }

         public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
         public virtual ICollection<PasswordReset> PasswordResets { get; set; } = null!;
    }
}