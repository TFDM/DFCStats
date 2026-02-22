using DFCStats.Domain.DTOs.Categories;

namespace DFCStats.Business.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets all categories from the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<CategoryDTO>> GetAllCategoriesAsync(string? sort = null);
    }
}