using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.Exceptions;
using DFCStats.Data.Entities;
using DFCStats.Business.MappingExtensions;
using Microsoft.EntityFrameworkCore;

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
        /// Searches for fixtures with optional filtering, sorting, and pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchSeason"></param>
        /// <param name="searchOpponent"></param>
        /// <param name="searchCompetition"></param>
        /// <param name="searchVenue"></param>
        /// <param name="searchOutcome"></param>
        /// <param name="searchCategory"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<(List<FixtureDTO>, int)> SearchForFixturesAsync(int page = 1, int pageSize = 50, string? searchSeason = null, string? searchOpponent = null, 
            string? searchCompetition = null, string? searchVenue = null, string? searchOutcome = null, string? searchCategory = null, string? sort = null)
        {
            // Ensure the page and page size are above not zero or negative
            page = (page < 1) ? 1 : page;
            pageSize = (pageSize < 1) ? 50 : pageSize;

            var fixtures = _dfcStatsDbContext.Fixtures
                .Include(f => f.Season)
                .Include(f => f.Club)
                .Include(f => f.Category)
                .Include(f => f.Venue)
                .AsNoTracking().AsQueryable();

            // Filter the records
            if (searchSeason != null)
                fixtures = fixtures.Where(f => f.SeasonId == Guid.Parse(searchSeason));

            if (searchOpponent != null)
                fixtures = fixtures.Where(f => f.ClubId == Guid.Parse(searchOpponent));

            if (searchCompetition != null)
                fixtures = fixtures.Where(f => f.Competition.Contains(searchCompetition));

            if (searchVenue != null)
                fixtures = fixtures.Where(f => f.VenueId == Guid.Parse(searchVenue));

            if (searchOutcome != null)
                fixtures = fixtures.Where(f => f.Outcome == searchOutcome);

            if (searchCategory != null)
                fixtures = fixtures.Where(f => f.CategoryId == Guid.Parse(searchCategory));

            
            // Sorts the records
            switch (sort)
            {
                case "date":
                    fixtures = fixtures.OrderBy(x => x.Date);
                    break;
                case "season_desc":
                    fixtures = fixtures.OrderByDescending(x => x.Season!.Description); //Season should never be null as its required - done to supress warning
                    break;
                case "season":
                    fixtures = fixtures.OrderBy(x => x.Season!.Description); //Season should never be null as its required - done to supress warning
					break;
                case "attendance_desc":
                    fixtures = fixtures.OrderByDescending(x => x.Attendance);
                    break;
                case "attendance":
                    fixtures = fixtures.OrderBy(x => x.Attendance);
                    break;
                default:
                    fixtures = fixtures.OrderByDescending(x => x.Date);
                    break;
            }

            // Counts the total number of records before any pagination is applied
			var totalItemCount = await fixtures.CountAsync();

            // Carries out the query
			var results = await fixtures.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
            
            // Return the fixtures (mapped to FixtureDTO) and the item count
            return (results.Select(n => n.MapToFixtureDTO()!).ToList(), totalItemCount);
        }

        /// <summary>
        /// Returns a fixture from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<FixtureDTO?> GetFixtureByIdAsync(Guid id)
        {
            var query = _dfcStatsDbContext.Fixtures.AsNoTracking().AsQueryable();
    
            // Includes the season, club, category and venue
            query = query.Include(f => f.Season);
            query = query.Include(f => f.Club);
            query = query.Include(f => f.Category);
            query = query.Include(f => f.Venue);

            // Run the query and map the entity to a DTO and return it
            var fixture = await query.FirstOrDefaultAsync(f => f.Id == id);
            return fixture?.MapToFixtureDTO();
        }

        /// <summary>
        /// Adds a new fixture to the database
        /// </summary>
        /// <param name="newFixtureDTO"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
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
        /// Updates a fixture in the database
        /// </summary>
        /// <param name="editFixtureDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FixtureDTO> UpdateFixtureAsync(EditFixtureDTO editFixtureDTO)
        {
            // Gets the fixture from the database
            var existingfixture = await _dfcStatsDbContext.Fixtures
                .FirstOrDefaultAsync(f => f.Id == editFixtureDTO.Id);

            // Check if the fixture exists in the database
            if (existingfixture == null)
                throw new DFCStatsException($"Fixture with id {editFixtureDTO.Id} not found");

            // Gets the season, club, venue and category from the database
            var season = await _seasonService.GetSeasonByIdAsync(editFixtureDTO.SeasonId);
            var club = await _clubService.GetClubByIdAsync(editFixtureDTO.ClubId);
            var venue = await _venueService.GetVenueByIdAsync(editFixtureDTO.VenueId);
            var category = await _categoryService.GetCategoryByIdAsync(editFixtureDTO.CategoryId);

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
            (int? darlingtonPenaltyScore, int? oppositionPenaltyScore) = SetPenaltyScores(editFixtureDTO.PenaltiesRequired, editFixtureDTO.DarlingtonPenaltyScore, editFixtureDTO.OppositionPenaltyScore);

            // Sets the outcome based on the scores and whether penalties were required
            string outcome = SetOutcome(editFixtureDTO.DarlingtonScore, editFixtureDTO.OppositionScore, editFixtureDTO.PenaltiesRequired, darlingtonPenaltyScore, oppositionPenaltyScore);

            // Update the fixture
            existingfixture.SeasonId = editFixtureDTO.SeasonId;
            existingfixture.Date = editFixtureDTO.Date;
            existingfixture.ClubId = editFixtureDTO.ClubId;
            existingfixture.CategoryId = editFixtureDTO.CategoryId;
            existingfixture.Competition = editFixtureDTO.Competition;
            existingfixture.VenueId = editFixtureDTO.VenueId;
            existingfixture.DarlingtonScore = editFixtureDTO.DarlingtonScore;
            existingfixture.OppositionScore = editFixtureDTO.OppositionScore;
            existingfixture.DarlingtonPenaltyScore = darlingtonPenaltyScore;
            existingfixture.OppositionPenaltyScore = oppositionPenaltyScore;
            existingfixture.Attendance = editFixtureDTO.Attendance;
            existingfixture.Outcome = outcome;
            existingfixture.Notes = editFixtureDTO.Notes;

            // Update the fixture in the database
            _dfcStatsDbContext.Fixtures.Update(existingfixture);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the updated fixture to a FixtureDTO and return it
            return existingfixture.MapToFixtureDTO()!;
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