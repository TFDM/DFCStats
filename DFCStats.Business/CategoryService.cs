using DFCStats.Business.Interfaces;
using DFCStats.Business.MappingExtensions;
using DFCStats.Data;
using DFCStats.Domain.DTOs.Categories;
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
        /// Gets a category by its id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CategoryDTO?> GetCategoryByIdAsync(Guid id)
        {
            // Get the category from the database
            var category = await _dfcStatsDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            // If not found, return null
            if (category == null)
                return null;

            // Map the entity to a DTO and return it
            return category.MapToCategoryDTO();
        }

        /// <summary>
        /// Gets all categories from the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync(string? sort = null)
        {
            // Get all the categories in the database
            var categories = _dfcStatsDbContext.Categories.AsQueryable();

            // Sort the records based on the sort parameter
            switch (sort)
            {
                case "orderNo_desc":
                    categories = categories.OrderByDescending(c => c.OrderNo);
                    break;
                case "orderNo":
                    categories = categories.OrderBy(c => c.OrderNo);
                    break;
                case "description_desc":
                    categories = categories.OrderByDescending(c => c.Description);
                    break;
                case "league_desc":
                    categories = categories.OrderByDescending(c => c.League);
                    break;
                case "league":
                    categories = categories.OrderBy(c => c.League);
                    break;
                case "cup_desc":
                    categories = categories.OrderByDescending(c => c.Cup);
                    break;
                case "cup":
                    categories = categories.OrderBy(c => c.Cup);
                    break;
                case "footballLeague_desc":
                    categories = categories.OrderByDescending(c => c.FootballLeague);
                    break;
                case "footballLeague":
                    categories = categories.OrderBy(c => c.FootballLeague);
                    break;
                case "nonLeague_desc":
                    categories = categories.OrderByDescending(c => c.NonLeague);
                    break;
                case "nonLeague":
                    categories = categories.OrderBy(c => c.NonLeague);
                    break;
                case "playOff_desc":
                    categories = categories.OrderByDescending(c => c.PlayOff);
                    break;
                case "playOff":
                    categories = categories.OrderBy(c => c.PlayOff);
                    break;
                default:
                    categories = categories.OrderBy(c => c.Description);
                    break;
            }

            // Map the categories to CategoryDTOs and return them
            return await categories.Select(c => c.MapToCategoryDTO()!).ToListAsync();
        }
    }
}