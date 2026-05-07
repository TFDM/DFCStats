using DFCStats.Domain.DTOs.Managers;

namespace DFCStats.Business.Interfaces
{
    public interface IManagerService
    {
        /// <summary>
        /// Returns a list of all the management records paginated. Also allows filtering out of caretaker records
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includeCaretakers"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<(List<ManagementRecordDTO>, int)> GetManagementRecordsWithPaginationAsync(int page = 1, int pageSize = 50, bool includeCaretakers = true, string? sort = null);

        /// <summary>
        /// Gets the management records for a given person id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<List<ManagementRecordDTO>> GetManagementRecordsByPersonIdAsync(Guid personId);
    }
}