using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Users;
using DFCStats.Domain.DTOs.Roles;

namespace DFCStats.Business.MappingExtensions
{
    public static class UserMappingExtensions
    {
        /// <summary>
        /// Maps a User entity to a UserDTO
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserDTO? MapToUserDTO(this User user)
        {
            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                AllowLogin = user.AllowLogin,
                Roles = user.UserRoles?
                    .Select(ur => ur.Role?.MapToRoleDTO())
                    .OfType<RoleDTO>()
                    .ToList()
            };
        }
    }
}