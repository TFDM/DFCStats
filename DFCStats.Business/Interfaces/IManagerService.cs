using DFCStats.Domain.DTOs.Managers;

namespace DFCStats.Business.Interfaces
{
    [Flags]
    public enum ManagerIncludes
    {
        // If new flags are required double the previous number
        None = 0,
        Person = 1,
        Nationality = 2,
        All = Person | Nationality
    }

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

        /// <summary>
        /// Gets a management record by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<ManagementRecordDTO?> GetManagementRecordByIdAsync(Guid id, ManagerIncludes includes = ManagerIncludes.None);

        /// <summary>
        /// Adds a management record to the database
        /// </summary>
        /// <param name="newManagementRecordDTO"></param>
        /// <returns></returns>
        Task<ManagementRecordDTO> AddManagerRecordAsync(ManagementRecordDTO newManagementRecordDTO);
    
        /// <summary>
        /// Updates a management record in the database
        /// </summary>
        /// <param name="editManagerRecordDTO"></param>
        /// <returns></returns>
        Task<ManagementRecordDTO> UpdateManagerRecordAsync(ManagementRecordDTO editManagerRecordDTO);
    }
}