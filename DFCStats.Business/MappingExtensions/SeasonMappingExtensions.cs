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
                    }).ToList(),
                Fixtures = season.Fixtures?
                    .Select(f => new DFCStats.Domain.DTOs.Seasons.Fixture
                    {
                        Id = f.Id,
                        SeasonId = f.SeasonId,
                        Season = f.Season!.Description,
                        Date = f.Date,
                        ClubId = f.ClubId,
                        Club = f.Club!.Name,
                        CategoryId = f.CategoryId,
                        Category = f.Category!.Description,
                        Competition = f.Competition,
                        VenueId = f.VenueId,
                        Venue = f.Venue!.Description,
                        VenueShort = f.Venue.ShortDescription,
                        Scoreline = f.Scoreline(),
                        Teams = f.Teams(),
                        TeamsAndScores = f.TeamsAndScores(),
                        PenaltiesRequired = f.PenaltiesRequired(),
                        PenaltyScoreline = f.PenaltyScoreline(),
                        PenaltyScoreWithOutcome = f.PenaltyScoreWithOutomce(),
                        Outcome = f.Outcome,
                        Attendance = f.Attendance
                    }).ToList() ?? new()
            };
        }
    }
}

