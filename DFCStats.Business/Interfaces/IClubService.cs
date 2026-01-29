using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface IClubService
    {
        /// <summary>
        /// Gets all the clubs from the database
        /// </summary>
        /// <returns></returns>
        Task<List<ClubDTO>> GetAllClubsAsync();
        
        /// <summary>
        /// Adds a club to the database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AddClub(ClubDTO dto);
    }
}

