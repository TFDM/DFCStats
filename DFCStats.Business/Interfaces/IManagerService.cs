using DFCStats.Domain.DTOs.Managers;

namespace DFCStats.Business.Interfaces
{
    public interface IManagerService
    {
        /// <summary>
        /// Gets the management records for a given person id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<List<ManagementRecordDTO>> GetManagementRecordsByPersonIdAsync(Guid personId);
    }
}