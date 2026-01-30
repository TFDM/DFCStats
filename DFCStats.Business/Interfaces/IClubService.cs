using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface IClubService
    {
        /// <summary>
        /// Check to see if a club name is already in use 
        /// Will return true if it is in use otherwise false
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> IsClubNameInUse(string name);

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

