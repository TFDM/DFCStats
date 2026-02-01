using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs;

namespace DFCStats.Business.MappingExtensions
{
    public static class SeasonMappingExtensions
    {
        /// <summary>
        /// Maps a Season entity to a SeasonDTO
        /// </summary>
        /// <param name="season"></param>
        /// <returns></returns>
        public static SeasonDTO? MapToSeasonDTO(this Season season)
        {
            if (season == null)
                return null;

            return new SeasonDTO
            {
                Id = season.Id,
                Description = season.Description
            };
        }
    }
}