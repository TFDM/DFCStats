using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs.Fixtures;

namespace DFCStats.Business
{
    public class FixtureService : IFixtureService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;
        private readonly ISeasonService _seasonService;
        private readonly IClubService _clubService;

        public FixtureService(DFCStatsDBContext dFCStatsDBContext, ISeasonService seasonService, IClubService clubService)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
            _seasonService = seasonService;
            _clubService = clubService;
        }

        /// <summary>
        /// Adds a new fixture to the database
        /// </summary>
        /// <param name="newFixtureDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FixtureDTO> AddFixtureAsync(NewFixtureDTO newFixtureDTO)
        {
            //Gets the season
            var season = await _seasonService.GetSeasonByIdAsync(newFixtureDTO.SeasonId);
            var club = await _clubService.GetClubByIdAsync(newFixtureDTO.ClubId);
            //Venue
            //Category

            throw new NotImplementedException();
        }


    }
}