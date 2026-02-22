using DFCStats.Data.Entities;
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
                PeopleAttachedToSeason = season.PersonSeasons
                    .Select(p => new PersonAttachedToSeason
                    {
                        Id = p.Id,
                        PersonId = p.PersonId,
                        FirstName = p.Person?.FirstName ?? string.Empty,
                        LastName = p.Person?.LastName ?? string.Empty,
                        LastNameFirstName = $"{p.Person?.LastName}, {p.Person?.FirstName}"
                    }).ToList()
            };
        }
    }
}