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
        /// Returns a list of all the management records paginated. Also allows filtering out of caretaker records
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includeCaretakers"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<(List<ManagementRecordDTO>, int)> GetManagementRecordsWithPaginationAsync(int page = 1, int pageSize = 50, bool includeCaretakers = true, string? sort = null)
        {
            // Ensure the page and page size are above not zero or negative
            page = (page < 1) ? 1 : page;
            pageSize = (pageSize < 1) ? 50 : pageSize;

            var managementRecords = _dfcStatsDbContext.View_Managers.AsNoTracking().AsQueryable();

            // Filter out caretaker records if includeCaretakers is false
            if (!includeCaretakers) 
                managementRecords = managementRecords.Where(m => !m.IsCareTaker);

            // Sorts the records
            switch (sort)
            {
                case "startDate":
                    managementRecords = managementRecords.OrderBy(x => x.StartDate);
                    break;
                case "endDate_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.AltEndDate);
                    break;
                case "endDate":
                    managementRecords = managementRecords.OrderBy(x => x.AltEndDate);
                    break;
                case "managerName_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.LastNameFirstName);
                    break;
                case "managerName":
                    managementRecords = managementRecords.OrderBy(x => x.LastNameFirstName);
                    break;
                case "nationality_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.Nationality);
                    break;
                case "nationality":
                    managementRecords = managementRecords.OrderBy(x => x.Nationality);
                    break;
                case "gamesManaged_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.GamesManaged);
                    break;
                case "gamesManaged":
                    managementRecords = managementRecords.OrderBy(x => x.GamesManaged);
                    break;
                case "won_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.Won);
                    break;
                case "won":
                    managementRecords = managementRecords.OrderBy(x => x.Won);
                    break;
                case "drawn_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.Drawn);
                    break;
                case "drawn":
                    managementRecords = managementRecords.OrderBy(x => x.Drawn);
                    break;
                case "lost_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.Lost);
                    break;
                case "lost":
                    managementRecords = managementRecords.OrderBy(x => x.Lost);
                    break;
                case "goalsFor_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.GoalsFor);
                    break;
                case "goalsFor":
                    managementRecords = managementRecords.OrderBy(x => x.GoalsFor);
                    break;
                case "goalsAgainst_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.GoalsAg);
                    break;
                case "goalsAgainst":
                    managementRecords = managementRecords.OrderBy(x => x.GoalsAg);
                    break;
                case "timeInCharge_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.DaysInCharge);
                    break;
                case "timeInCharge":
                    managementRecords = managementRecords.OrderBy(x => x.DaysInCharge);
                    break;
                case "winsPercentage_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.WinPercentage);
                    break;
                case "winsPercentage":
                    managementRecords = managementRecords.OrderBy(x => x.WinPercentage);
                    break;
                default:
                    managementRecords = managementRecords.OrderByDescending(x => x.StartDate);
                    break;
            }

            // Counts the total number of records before any pagination is applied
			var totalItemCount = await managementRecords.CountAsync();

            // Carries out the query
            var results = await managementRecords.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            // Return the management records (mapped to MapToManagementRecordDTO) and the item count
            return (results.Select(m => m.MapToManagementRecordDTO()!).ToList(), totalItemCount);
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