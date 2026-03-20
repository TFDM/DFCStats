using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Fixtures;

namespace DFCStats.Business.MappingExtensions
{
    public static class FixtureMappingExtensions
    {
        /// <summary>
        /// Maps a club entity to a ClubDTO
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        public static FixtureDTO? MapToFixtureDTO(this Fixture fixture)
        {
            if (fixture == null)
                return null;

            return new FixtureDTO
            {
                Id = fixture.Id,
                SeasonId = fixture.SeasonId,
                Season = fixture.Season!.Description,
                Date = fixture.Date
            };
        }
    }
}