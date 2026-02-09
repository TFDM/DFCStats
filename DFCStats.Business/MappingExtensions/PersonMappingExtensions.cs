using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs;

namespace DFCStats.Business.MappingExtensions
{
    public static class PersonMappingExtensions
    {
        /// <summary>
        /// Maps a Person entity to a PersonDTO
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static PersonDTO? MapToPersonDTO(this Person person)
        {
            if (person == null)
                return null;

            return new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                LastNameFirstName = $"{person.LastName}, {person.FirstName}",
                DateOfBirth = person.DateOfBirth,
                NationalityId = person.NationalityId,
                Nationality = person.Nationality?.Name,
                NationalityIcon = person.Nationality?.Icon,
                Biography = person.Biography,
                IsManager = person.IsManager,
                Seasons = person.PersonSeasons?
                    .Select(ps => new SeasonDTO
                    {
                        Id = ps.SeasonId,
                        Description = ps.Season?.Description ?? string.Empty
                    }).ToList()
            };
        }
        
    }
}