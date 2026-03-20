using DFCStats.Domain.DTOs.Categories;

namespace DFCStats.Business.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets a category by its id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CategoryDTO?> GetCategoryByIdAsync(Guid id);

        /// <summary>
        /// Gets all categories from the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<CategoryDTO>> GetAllCategoriesAsync(string? sort = null);
    }
}