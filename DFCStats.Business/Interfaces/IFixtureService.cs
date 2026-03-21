using DFCStats.Domain.DTOs.Fixtures;

namespace DFCStats.Business.Interfaces
{
    public interface IFixtureService
    {
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