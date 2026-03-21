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
        All = PeopleAttachedToSeason | Fixtures
    }

    public interface ISeasonService
    {
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
    }
}