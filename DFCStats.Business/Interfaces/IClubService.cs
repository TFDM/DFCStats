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
        /// Returns a list of all the club records paginated. Also allows filtering out of caretaker records
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchName"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<(List<ClubDTO>, int)> SearchForClubsAsync(int page = 1, int pageSize = 50, string? searchName = null, string? sort = null);

        /// <summary>
        /// Adds a club to the database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ClubDTO> AddClubAsync(ClubDTO clubDTO);
    
        /// <summary>
        /// Updates a club in the database
        /// </summary>
        /// <param name="editClubDTO"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        Task<ClubDTO> UpdateClubAsync(ClubDTO editClubDTO);
    }
}

