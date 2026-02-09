using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface IPersonService
    {
        /// <summary>
        /// Returns a person from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PersonDTO?> GetPersonByIdAsync(Guid id);

        /// <summary>
        /// Adds a new person to the database
        /// </summary>
        /// <param name="newPersonDTO"></param>
        /// <returns></returns>
        Task<PersonDTO> AddPersonAsync(NewPersonDTO newPersonDTO);
    }
}