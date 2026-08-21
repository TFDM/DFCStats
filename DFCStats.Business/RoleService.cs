using DFCStats.Business.Interfaces;
using DFCStats.Business.MappingExtensions;
using DFCStats.Data;
using DFCStats.Domain.DTOs.Roles;
using Microsoft.EntityFrameworkCore;

namespace DFCStats.Business
{
    public class RoleService : IRoleService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public RoleService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Get all the roles from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleDTO>> GetAllRolesAsync(string? sort = null)
        {
            // Get all the roles in the database
            var roles = _dfcStatsDbContext.Roles.AsNoTracking().AsQueryable();

            // Sort the records based on the sort paramter
            switch (sort)
            {
                case "name_desc":
                    roles = roles.OrderByDescending(r => r.Name);
                    break;
                default:
                    roles = roles.OrderBy(r => r.Name);
                    break;
            }

            // Map the roles to RoleDTO and return them
            return await roles.Select(r => r.MapToRoleDTO()!).ToListAsync();
        }

        /// <summary>
        /// Finds role IDs that do not exist in the database.
        /// </summary>
        /// <param name="roleIds">The role IDs to check.</param>
        /// <returns>The role IDs that were not found.</returns>
        public async Task<List<Guid>> GetMissingRoleIdsAsync(List<Guid> roleIds)
        {
            // Remove duplicate Ids so each role is checked only once.
            var requestedRoleIds = roleIds
                .Distinct()
                .ToList();

            // Query only the matching IDs instead of loading complete role records.
            var existingRoleIds = await _dfcStatsDbContext.Roles
                .Where(role => requestedRoleIds.Contains(role.Id))
                .Select(role => role.Id)
                .ToListAsync();

            // IDs requested by the user but absent from the database are invalid.
            return requestedRoleIds
                .Except(existingRoleIds)
                .ToList();
        }
    }
}