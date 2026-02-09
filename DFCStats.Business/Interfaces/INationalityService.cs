using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface INationalityService
    {
        /// <summary>
        /// Returns a nationality from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<NationalityDTO?> GetNationalityByIdAsync(Guid id);

        /// <summary>
        /// Returns a nationality from the database using the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<NationalityDTO?> GetNationalityByNameAsync(string name);
        
        /// <summary>
        /// Gets all the nationalities from the database with optional sorting
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<NationalityDTO>> GetAllNationalitiesAsync(string? sort = null);

        /// <summary>
        /// Searches for nationalities with optional filtering, sorting, and pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchCountry"></param>
        /// <param name="searchNationality"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<(List<NationalityDTO>, int)> SearchForNationalitiesAsync(int page = 1, int pageSize = 1, string? searchCountry = null, string? searchNationality = null, string? sort = null);
    
        /// <summary>
        /// Adds a nationality to the database
        /// </summary>
        /// <param name="nationalityDTO"></param>
        /// <returns></returns>
        Task<NationalityDTO> AddNationalityAsync(NewNationalityDTO newNationalityDTO);
    
        /// <summary>
        /// Updates a nationality in the database
        /// </summary>
        /// <param name="nationalityDTO"></param>
        /// <returns></returns>
        Task<NationalityDTO> UpdateNationalityAsync(EditNationalityDTO editNationalityDTO);
    }
}