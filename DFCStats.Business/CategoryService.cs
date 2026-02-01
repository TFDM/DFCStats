using DFCStats.Business.Interfaces;
using DFCStats.Business.MappingExtensions;
using DFCStats.Data;
using DFCStats.Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DFCStats.Business
{
    public class CategoryService : ICategoryService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public CategoryService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Gets all categories from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            // Gets all the categories
            var categories = await _dfcStatsDbContext.Categories.ToListAsync();

            // Map the categories to CategoryDTOs and return them
            return categories.Select(c => c.MapToCategoryDTO()!).ToList();
        }
    }
}