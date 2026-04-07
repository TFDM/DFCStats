using DFCStats.Domain.DTOs.People;
using DFCStats.Domain.DTOs.Fixtures;

namespace DFCStats.Business.Interfaces
{
    [Flags]
    public enum PersonIncludes
    {
        // If new flags are required double the previous number
        None = 0,
        Nationality = 1,
        Seasons = 2,
        Stats = 4,
        All = Nationality | Seasons | Stats
    }

    public interface IPersonService
    {
        /// <summary>
        /// Searches for people with optional filtering, sorting, and pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchFirstName"></param>
        /// <param name="searchLastName"></param>
        /// <param name="searchNationalityId"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<(List<PersonDTO>, int)> SearchForPeopleAsync(int page = 1, int pageSize = 50, string? searchFirstName = null, string? searchLastName = null, 
            string? searchNationalityId = null, string? sort = null);

        /// <summary>
        /// Returns a person from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<PersonDTO?> GetPersonByIdAsync(Guid id, PersonIncludes includes = PersonIncludes.None);

        /// <summary>
        /// Adds a new person to the database
        /// </summary>
        /// <param name="newPersonDTO"></param>
        /// <returns></returns>
        Task<PersonDTO> AddPersonAsync(NewPersonDTO newPersonDTO);

        /// <summary>
        /// Update the person in the database
        /// </summary>
        /// <param name="editPersonDTO"></param>
        /// <returns></returns>
        Task<PersonDTO> UpdatePersonAsync(EditPersonDTO editPersonDTO);

        /// <summary>
        /// Gets the fixtures a selected person appeared in for a selected season
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="seasonId"></param>
        /// <returns></returns>
        Task<List<ParticipationFixtureDTO>> GetParticipatedFixturesAsync(Guid personId, Guid seasonId);
    }
}