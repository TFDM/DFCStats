using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface INationalityService
    {
        /// <summary>
        /// Gets all the nationalities from the database
        /// </summary>
        /// <returns></returns>
        Task<List<NationalityDTO>> GetAllNationalitiesAsync();

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
    }
}