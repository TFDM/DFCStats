using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.People;
using DFCStats.Domain.DTOs.Seasons;

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
                    .Select(ps => ps.Season?.MapToSeasonShortDTO())
                    .OfType<SeasonShortDTO>()
                    .ToList(),
                TotalApps = person.Participation?.ToList().TotalAppearances(),
                TotalGoals = person.Participation?.ToList().TotalGoals(),
                TotalRedCards = person.Participation?.ToList().TotalRedCards(),
                TotalStarts = person.Participation?.ToList().TotalStarts(),
                TotalSubs = person.Participation?.ToList().TotalSubs(),
                TotalLeagueStarts = person.Participation?.ToList().TotalLeagueStarts(),
                TotalLeagueSubs = person.Participation?.ToList().TotalLeagueSubs(),
                TotalLeagueGoals = person.Participation?.ToList().TotalLeagueGoals(),
                TotalPlayOffStarts = person.Participation?.ToList().TotalPlayOffStarts(),
                TotalPlayOffSubs = person.Participation?.ToList().TotalPlayOffSubs(),
                TotalPlayOffGoals = person.Participation?.ToList().TotalPlayOffGoals(),
                TotalCupStarts = person.Participation?.ToList().TotalCupStarts(),
                TotalCupSubs = person.Participation?.ToList().TotalCupSubs(),
                TotalCupGoals = person.Participation?.ToList().TotalPlayOffGoals(),
                Appearances = person.Participation?.ToList().MapToAppearanceDTO(),
            };
        }
        
    }
}
