using DFCStats.Domain.DTOs.Roles;

namespace DFCStats.Domain.DTOs.Users
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set;} = string.Empty;
        public bool AllowLogin { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public List<RoleDTO>? Roles { get; set; }
    }
}