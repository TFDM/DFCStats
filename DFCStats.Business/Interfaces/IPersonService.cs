using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    [Flags]
    public enum PersonIncludes
    {
        // If new flags are required double the previous number
        None = 0,
        Nationality = 1,
        Seasons = 2,
        All = Nationality | Seasons
    }

    public interface IPersonService
    {
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
    }
}