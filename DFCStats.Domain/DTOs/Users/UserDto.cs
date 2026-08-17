using DFCStats.Domain.DTOs.Roles;

namespace DFCStats.Domain.DTOs.Users
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set;} = string.Empty;
        public bool AllowLogin { get; set; }
        public List<RoleDTO>? Roles { get; set; }
    }
}