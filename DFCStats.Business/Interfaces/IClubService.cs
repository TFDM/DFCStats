using DFCStats.Domain.DTOs.Clubs;

namespace DFCStats.Business.Interfaces
{
    public interface IClubService
    {
        /// <summary>
        /// Gets a club by its id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ClubDTO?> GetClubByIdAsync(Guid id);

        /// <summary>
        /// Gets all the clubs from the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<ClubDTO>> GetAllClubsAsync(string? sort = null);
        
        /// <summary>
        /// Adds a club to the database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ClubDTO> AddClubAsync(ClubDTO clubDTO);
    }
}

