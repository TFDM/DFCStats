using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.DTOs.People;
using DFCStats.Domain.DTOs.Seasons;

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
                Description = season.Description,
                PeopleAttachedToSeason = season.PersonSeasons?
                    .Select(p => p.Person.MapToPersonDTO())
                    .OfType<PersonDTO>()
                    .ToList(),
                Fixtures = season.Fixtures?
                    .Select(f => f.MapToFixtureDTO())
                    .OfType<FixtureDTO>()
                    .ToList()
            };
        }

        /// <summary>
        /// Maps a Season entity to a SeasonShortDTO
        /// </summary>
        /// <param name="season"></param>
        /// <returns></returns>
        public static SeasonShortDTO? MapToSeasonShortDTO(this Season season)
        {
            if (season == null)
                return null;

            return new SeasonShortDTO
            {
                Id = season.Id,
                Description = season.Description
            };
        }
    }
}

