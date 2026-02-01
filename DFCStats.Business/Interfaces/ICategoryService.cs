using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets all categories from the database
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
    }
}