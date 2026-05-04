using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs.Managers;
using Microsoft.EntityFrameworkCore;
using DFCStats.Business.MappingExtensions;

namespace DFCStats.Business
{
    public class ManagerService : IManagerService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public ManagerService(DFCStatsDBContext dfcStatsDbContext)
        {
            _dfcStatsDbContext = dfcStatsDbContext;
        }

        /// <summary>
        /// Gets the management records for a given person id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<List<ManagementRecordDTO>> GetManagementRecordsByPersonIdAsync(Guid personId)
        {
            var managerRecords = _dfcStatsDbContext.View_Managers.AsNoTracking()
                .Where(m => m.PersonId == personId)
                .AsQueryable();

            // Map the View_Managers to the ManagementRecordDTO and return them
            return await managerRecords.Select(m => m.MapToManagementRecordDTO()!).ToListAsync();
        }
    }
}