using DFCStats.Domain.DTOs.Roles;

namespace DFCStats.Business.Interfaces
{
    public interface IRoleService
    {
        /// <summary>
        /// Get all the roles from the database
        /// </summary>
        /// <returns></returns>
        Task<List<RoleDTO>> GetAllRolesAsync(string? sort = null);

        /// <summary>
        /// Finds role IDs that do not exist in the database.
        /// </summary>
        /// <param name="roleIds">The role IDs to check.</param>
        /// <returns>The role IDs that were not found.</returns>
        Task<List<Guid>> GetMissingRoleIdsAsync(List<Guid> roleIds);
    }
}