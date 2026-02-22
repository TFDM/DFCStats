using DFCStats.Domain.DTOs.Fixtures;

namespace DFCStats.Business.Interfaces
{
    public interface IFixtureService
    {
        /// <summary>
        /// Adds a new fixture to the database
        /// </summary>
        /// <param name="newFixtureDTO"></param>
        /// <returns></returns>
        Task<FixtureDTO> AddFixtureAsync(NewFixtureDTO newFixtureDTO);
    }
}