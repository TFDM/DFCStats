using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs;

namespace DFCStats.Business.MappingExtensions
{
    public static class CategoryMappingExtensions
    {
        /// <summary>
        /// Maps a category entity to a CategoryDTO
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static CategoryDTO? MapToCategoryDTO(this Category category)
        {
            if (category == null)
                return null;

            return new CategoryDTO
            {
                Id = category.Id,
                Description = category.Description,
                League = category.League,
                Cup = category.Cup,
                FootballLeague = category.FootballLeague,
                NonLeague =  category.NonLeague,
                PlayOff = category.PlayOff,
                OrderNo = category.OrderNo
            };
        }
    }
}