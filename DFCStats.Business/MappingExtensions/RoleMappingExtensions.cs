using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Roles;

namespace DFCStats.Business.MappingExtensions
{
    public static class RoleMappingExtensions
    {
        /// <summary>
        /// Maps a Role entity to a RoleDTO
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static RoleDTO? MapToRoleDTO(this Role role)
        {
            if (role == null)
                return null;

            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
        }

    }
}