using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class PasswordReset
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [MaxLength(64)]
        public string Token { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }
        public DateTime? InvalidatedAt { get; set; }
        public DateTime? UsedAt { get; set; }
        [Required]
        [MaxLength(45)]
        public string RequesterIpAddress { get; set; } = string.Empty;
        public virtual User? User { get; set; }
    }
}