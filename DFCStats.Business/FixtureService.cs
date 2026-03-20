using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.Exceptions;
using DFCStats.Data.Entities;
using DFCStats.Business.MappingExtensions;

namespace DFCStats.Business
{
    public class FixtureService : IFixtureService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;
        private readonly ISeasonService _seasonService;
        private readonly IClubService _clubService;
        private readonly IVenueService _venueService;
        private readonly ICategoryService _categoryService;

        public FixtureService(DFCStatsDBContext dFCStatsDBContext, ISeasonService seasonService, IClubService clubService, IVenueService venueService, ICategoryService categoryService)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
            _seasonService = seasonService;
            _clubService = clubService;
            _venueService = venueService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Adds a new fixture to the database
        /// </summary>
        /// <param name="newFixtureDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FixtureDTO> AddFixtureAsync(NewFixtureDTO newFixtureDTO)
        {
            // Gets the season, club, venue and category from the database
            var season = await _seasonService.GetSeasonByIdAsync(newFixtureDTO.SeasonId);
            var club = await _clubService.GetClubByIdAsync(newFixtureDTO.ClubId);
            var venue = await _venueService.GetVenueByIdAsync(newFixtureDTO.VenueId);
            var category = await _categoryService.GetCategoryByIdAsync(newFixtureDTO.CategoryId);

            // Check that the season, club, venue and category all exist in the database, if not throw an exception
            if (season == null)
                throw new DFCStatsException("Season wasn't found in the database");

            if (club == null)
                throw new DFCStatsException("Club wasn't found in the database");

            if (venue == null)
                throw new DFCStatsException("Venue wasn't found in the database");

            if (category == null)
                throw new DFCStatsException("Category wasn't found in the database");

            // Sets the penalty scores and the outcome of the fixture
            (int? darlingtonPenaltyScore, int? oppositionPenaltyScore) = SetPenaltyScores(newFixtureDTO.PenaltiesRequired, newFixtureDTO.DarlingtonPenaltyScore, newFixtureDTO.OppositionPenaltyScore);

            // Sets the outcome based on the scores and whether penalties were required
            string outcome = SetOutcome(newFixtureDTO.DarlingtonScore, newFixtureDTO.OppositionScore, newFixtureDTO.PenaltiesRequired, darlingtonPenaltyScore, oppositionPenaltyScore);

            // Create the fixture using the dto
            var fixture = new Fixture()
            {
                SeasonId = newFixtureDTO.SeasonId,
                Date = newFixtureDTO.Date,
                ClubId = newFixtureDTO.ClubId,
                CategoryId = newFixtureDTO.CategoryId,
                Competition = newFixtureDTO.Competition,
                VenueId = newFixtureDTO.VenueId,
                DarlingtonScore = newFixtureDTO.DarlingtonScore,
                OppositionScore = newFixtureDTO.OppositionScore,
                DarlingtonPenaltyScore = darlingtonPenaltyScore,
                OppositionPenaltyScore = oppositionPenaltyScore,
                Attendance = newFixtureDTO.Attendance,
                Outcome = outcome,
                Notes = newFixtureDTO.Notes
            };

            await _dfcStatsDbContext.Fixtures.AddAsync(fixture);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created fixture to a FixtureDTO and return it
            return fixture.MapToFixtureDTO()!;
        }

        /// <summary>
        /// Ensures the penalty scores are set to null if penalties were not required.
        /// This ensures thats if the penalty scores were set and then then afterwards 
        /// the user set penalties to not be required that the penalty scores are set 
        /// to null
        /// </summary>
        /// <param name="penaltiesRequired"></param>
        /// <param name="darlingtonPenaltyScore"></param>
        /// <param name="oppositionPenaltyScore"></param>
        /// <returns></returns>
        private (int?, int?) SetPenaltyScores(bool penaltiesRequired, int? darlingtonPenaltyScore, int? oppositionPenaltyScore)
        {
            if (!penaltiesRequired)
                // If penalties are not required, return nulls
                return (null, null);

            // If penalties are required, return the provided scores
            return (darlingtonPenaltyScore, oppositionPenaltyScore);
        }

        /// <summary>
        /// Sets the correct outcome for the fixture based on the scores
        /// </summary>
        /// <param name="darlingtonScore"></param>
        /// <param name="oppositionScore"></param>
        /// <param name="penaltiesRequired"></param>
        /// <param name="darlingtonPenaltyScore"></param>
        /// <param name="oppositionPenaltyScore"></param>
        /// <returns></returns>
        private string SetOutcome(int darlingtonScore, int oppositionScore, bool penaltiesRequired, int? darlingtonPenaltyScore, int? oppositionPenaltyScore)
        {
            //Default the outcome to a win
            string outcome = "W";

            if (penaltiesRequired)
            {
                //Penalties were required - check to see if the Darlington penalty score
                //is less than the opposition penalty score - penalty shootouts don't end in
                //draws so its either a win or a defeat
                if (darlingtonPenaltyScore < oppositionPenaltyScore)
                {
                    outcome = "L"; //Sets the outcome to a loss
                }
            }
            else
            {
                //Penalties not required - the fixture has already been defeaulted to a win
                //so the only thing that needs to be checked is are the scores either equal or 
                //have the opposition scored more
                if (darlingtonScore == oppositionScore)
                {
                    outcome = "D"; //Sets the outcome to a draw
                }
                else if (darlingtonScore < oppositionScore)
                {
                    outcome = "L"; //Sets the outcome to a loss
                }
            }

            return outcome;
        }

    }
}