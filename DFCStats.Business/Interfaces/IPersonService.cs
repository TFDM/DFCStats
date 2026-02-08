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
    }
}