using DFCStats.Domain.DTOs.Fixtures;

namespace DFCStats.Business.Interfaces
{
    public interface IFixtureService
    {
        /// <summary>
        /// Searches for fixtures with optional filtering, sorting, and pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchSeason"></param>
        /// <param name="searchOpponent"></param>
        /// <param name="searchCompetition"></param>
        /// <param name="searchVenue"></param>
        /// <param name="searchOutcome"></param>
        /// <param name="searchCategory"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<(List<FixtureDTO>, int)> SearchForFixturesAsync(int page = 1, int pageSize = 50, string? searchSeason = null, string? searchOpponent = null, 
            string? searchCompetition = null, string? searchVenue = null, string? searchOutcome = null, string? searchCategory = null, string? sort = null);

        /// <summary>
        /// Returns a fixture from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<FixtureDTO?> GetFixtureByIdAsync(Guid id);

        /// <summary>
        /// Adds a new fixture to the database
        /// </summary>
        /// <param name="newFixtureDTO"></param>
        /// <returns></returns>
        Task<FixtureDTO> AddFixtureAsync(NewFixtureDTO newFixtureDTO);

        /// <summary>
        /// Updates a fixture in the database
        /// </summary>
        /// <param name="editFixtureDTO"></param>
        /// <returns></returns>
        Task<FixtureDTO> UpdateFixtureAsync(EditFixtureDTO editFixtureDTO);
    }
}