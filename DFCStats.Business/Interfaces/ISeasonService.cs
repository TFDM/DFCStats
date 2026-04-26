using DFCStats.Domain.DTOs.Seasons;

namespace DFCStats.Business.Interfaces
{
    [Flags]
    public enum SeasonIncludes
    {
        // If new flags are required double the previous number
        None = 0,
        PeopleAttachedToSeason = 1,
        Fixtures = 2,
        Appearances = 4,
        All = PeopleAttachedToSeason | Fixtures | Appearances
    }

    public interface ISeasonService
    {
        /// <summary>
        /// Returns a list of all the seasons paginated
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<(List<SeasonDTO>, int)> GetAllSeasonsWithPaginationAsync(int page = 1, int pageSize = 50, string? sort = null);

        /// <summary>
        /// Returns a season from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SeasonDTO?> GetSeasonByIdAsync(Guid id, SeasonIncludes includes = SeasonIncludes.None);

        /// <summary>
        /// Returns a list of all the seasons in the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<SeasonDTO>> GetAllSeasonsAsync(string? sort = null);

        /// <summary>
        /// Adds a season to the database
        /// </summary>
        /// <param name="seasonDTO"></param>
        /// <returns></returns>
        Task<SeasonDTO> AddSeasonAsync(SeasonDTO seasonDTO);

        /// <summary>
        /// Updates a season in the database
        /// </summary>
        /// <param name="editSeasonDTO"></param>
        /// <returns></returns>
        Task<SeasonDTO> UpdateSeasonAsync(SeasonDTO editSeasonDTO);
    }
}