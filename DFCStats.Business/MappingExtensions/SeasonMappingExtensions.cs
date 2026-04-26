using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Appearances;
using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.DTOs.People;
using DFCStats.Domain.DTOs.Seasons;

namespace DFCStats.Business.MappingExtensions
{
    public static class SeasonMappingExtensions
    {
        /// <summary>
        /// Maps a View_Seasons entity to a SeasonDTO
        /// </summary>
        /// <param name="season"></param>
        /// <returns></returns>
        public static SeasonDTO? MapToSeasonDTO(this View_Seasons season)
        {
            if (season == null)
                return null;

            return new SeasonDTO
            {
                Id = season.Id,
                Description = season.Description,
                GamesPlayed = season.GamesPlayed,
                GamesWon = season.Wins,
                GamesDrawn = season.Draws,
                GamesLost = season.Loses,
                TotalPlayersUed = season.PlayersUsed,
                WinPercentage = season.WinPercentage,
                AverageHomeAttendance = season.AverageHomeAttendance,
                HighestHomeAttendance = season.HighestHomeAttendance
            };
        }

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
                    .ToList(),
                Appearances = season.Fixtures?
                    .SelectMany(f => f.Participants ?? new List<Participation>())  // Flatten all participations from fixtures
                    .GroupBy(p => p.PersonId)  // Group by person
                    .SelectMany(g => g.ToList().MapToAppearanceDTO() ?? new List<SeasonalAppearanceDTO>())  // Map each group and flatten results
                    .ToList(),
                GamesPlayed = season.Fixtures?.Count(),
                GamesWon = season.Fixtures?.Where(f => f.Outcome == "W").Count(),
                GamesDrawn = season.Fixtures?.Where(f => f.Outcome == "D").Count(),
                GamesLost = season.Fixtures?.Where(f => f.Outcome == "L").Count(),
                TotalPlayersUed = season.Fixtures?.Where(f => f.Category?.Description != "Friendly").SelectMany(p => p.Participants.Select(p => p.PersonId)).Distinct().Count(),
                WinPercentage = season.Fixtures != null && season.Fixtures.Any() ? (decimal)season.Fixtures.Count(f => f.Outcome == "W")  / season.Fixtures.Count() * 100m : 0m,
                AverageHomeAttendance = (int)Math.Floor(season.Fixtures?
                    .Where(f => f.Category?.Description != "Friendly" && 
                                f.Venue?.ShortDescription == "H" && 
                                f.Attendance.HasValue)
                    .Select(f => f.Attendance!.Value)
                    .DefaultIfEmpty(0)
                    .Average() ?? 0),
                HighestHomeAttendance = season.Fixtures?
                    .Where(f => f.Category?.Description != "Friendly" && 
                                f.Venue?.ShortDescription == "H" && 
                                f.Attendance.HasValue)
                    .Select(f => f.Attendance!.Value)
                    .DefaultIfEmpty(0)
                    .Max() ?? 0
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

