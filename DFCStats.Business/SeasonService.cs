using DFCStats.Data;
using DFCStats.Data.Entities;
using DFCStats.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using DFCStats.Domain.DTOs.Seasons;
using DFCStats.Domain.Exceptions;
using DFCStats.Business.MappingExtensions;

namespace DFCStats.Business
{
    public class SeasonService : ISeasonService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public SeasonService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Returns a list of all the seasons paginated
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<(List<SeasonDTO>, int)> GetAllSeasonsWithPaginationAsync(int page = 1, int pageSize = 50, string? sort = null)
        {
            // Ensure the page and page size are above not zero or negative
            page = (page < 1) ? 1 : page;
            pageSize = (pageSize < 1) ? 50 : pageSize;

            var seasons = _dfcStatsDbContext.View_Seasons.AsNoTracking().AsQueryable();

            // Sorts the records
            switch (sort)
            {
                case "description_desc":
                    seasons = seasons.OrderByDescending(x => x.Description);
                    break;
                case "gamesplayed":
                    seasons = seasons.OrderBy(x => x.GamesPlayed);
                    break;
                case "gamesplayed_desc":
                    seasons = seasons.OrderByDescending(x => x.GamesPlayed);
                    break;
                case "wins_desc":
                    seasons = seasons.OrderByDescending(x => x.Wins);
                    break;
                case "wins":
                    seasons = seasons.OrderBy(x => x.Wins);
                    break;
                case "draws_desc":
                    seasons = seasons.OrderByDescending(x => x.Draws);
                    break;
                case "draws":
                    seasons = seasons.OrderBy(x => x.Draws);
                    break;
                case "loses_desc":
                    seasons = seasons.OrderByDescending(x => x.Loses);
                    break;
                case "loses":
                    seasons = seasons.OrderBy(x => x.Loses);
                    break;
                case "averagehomeatt_desc":
                    seasons = seasons.OrderByDescending(x => x.AverageHomeAttendance);
                    break;
                case "averagehomeatt":
                    seasons = seasons.OrderBy(x => x.AverageHomeAttendance);
                    break;
                case "highesthomeatt_desc":
                    seasons = seasons.OrderByDescending(x => x.HighestHomeAttendance);
                    break;
                case "highesthomeatt":
                    seasons = seasons.OrderBy(x => x.HighestHomeAttendance);
                    break;
                case "playersused":
                    seasons = seasons.OrderBy(x => x.PlayersUsed);
                    break;
                case "playersused_desc":
                    seasons = seasons.OrderByDescending(x => x.PlayersUsed);
                    break;
                case "winpercentage":
                    seasons = seasons.OrderBy(x => x.WinPercentage);
                    break;
                case "winpercentage_desc":
                    seasons = seasons.OrderByDescending(x => x.WinPercentage);
                    break;
                default:
                    seasons = seasons.OrderBy(x => x.Description);
                    break;
            }
         
            // Counts the total number of records before any pagination is applied
			var totalItemCount = await seasons.CountAsync();

            // Carries out the query
            var results = await seasons.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            // Return the fixtures (mapped to FixtureDTO) and the item count
            return (results.Select(n => n.MapToSeasonDTO()!).ToList(), totalItemCount);
        }

        /// <summary>
        /// Returns a season from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SeasonDTO?> GetSeasonByIdAsync(Guid id, SeasonIncludes includes = SeasonIncludes.None)
        {
            var query = _dfcStatsDbContext.Seasons.AsNoTracking().AsQueryable();

            // Includes the people attached to the season and then the people themselves
            if (includes.HasFlag(SeasonIncludes.PeopleAttachedToSeason))
                query = query.Include(s => s.PersonSeasons).ThenInclude(ps => ps.Person);

            // Include the fixtures and then the related entities for the fixtures
            if (includes.HasFlag(SeasonIncludes.Fixtures))
                query = query.Include(s => s.Fixtures)
                    .ThenInclude(f => f.Club)
                    .Include(s => s.Fixtures)
                    .ThenInclude(f => f.Category)
                    .Include(s => s.Fixtures)
                    .ThenInclude(f => f.Venue);

            // Include the appearance data by including the participation data, fixtures and categories
            if (includes.HasFlag(SeasonIncludes.Appearances))
                query = query.Include(s => s.Fixtures)
                    .ThenInclude(p => p.Participants).ThenInclude(p => p.Person)
                    .Include(s => s.Fixtures).ThenInclude(f => f.Category);
        
            // Run the query and map the entity to a DTO and return it
            var season = await query.FirstOrDefaultAsync(s => s.Id == id);
            return season?.MapToSeasonDTO();
        }

        /// <summary>
        /// Check to see if a season description is already in use 
        /// Will return true if it is in use otherwise false
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private async Task<bool> IsSeasonDescriptionInUseAsync(string description, Guid? currentId = null)
        {
            var normalizedDescription = description.ToLower().Trim();

            return await _dfcStatsDbContext.Seasons
                .AsNoTracking()
                .AnyAsync(s => s.Description.ToLower().Trim() == normalizedDescription && (currentId == null || s.Id != currentId));
        }

        /// <summary>
        /// Returns a list of all the seasons in the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<List<SeasonDTO>> GetAllSeasonsAsync(string? sort = null)
        {
            // Get all the seasons in the database
            var seasons = _dfcStatsDbContext.Seasons.AsNoTracking().AsQueryable();

            // Sort the records based on the sort parameter
            switch (sort)
            {
                case "description_desc":
                    seasons = seasons.OrderByDescending(s => s.Description);
                    break;
                default:
                    seasons = seasons.OrderBy(s => s.Description);
                    break;
            }

            // Map the seasons to SeasonDTOs and return them
            return await seasons.Select(s => s.MapToSeasonDTO()!).ToListAsync();
        }

        /// <summary>
        /// Adds a season to the database
        /// </summary>
        /// <param name="seasonDTO"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        public async Task<SeasonDTO> AddSeasonAsync(SeasonDTO seasonDTO)
        {
            // Check to see if the season description is already in use
            if(await IsSeasonDescriptionInUseAsync(seasonDTO.Description))
                throw new DFCStatsException($"{seasonDTO.Description} is already in use" );

            // Create the season using the dto
            var season = new Season() { Description = seasonDTO.Description };

            // Add the season to the database and save the changes
            await _dfcStatsDbContext.Seasons.AddAsync(season);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created season to a SeasonDTO and return it
            return season.MapToSeasonDTO()!;
        }

        /// <summary>
        /// Updates a season in the database
        /// </summary>
        /// <param name="editSeasonDTO"></param>
        /// <returns></returns>
        public async Task<SeasonDTO> UpdateSeasonAsync(SeasonDTO editSeasonDTO)
        {
            // Gets the season from the database
            var existingSeason = await _dfcStatsDbContext.Seasons
                .FirstOrDefaultAsync(s => s.Id == editSeasonDTO.Id);

            // Check if the season exists in the database
            if (existingSeason == null)
                throw new DFCStatsException($"Season with id {editSeasonDTO.Id} not found");

            // Check to see if the season description is already in use
            if(await IsSeasonDescriptionInUseAsync(editSeasonDTO.Description, editSeasonDTO.Id))
                throw new DFCStatsException($"{editSeasonDTO.Description} is already in use" );

            // Update the description of the season
            existingSeason.Description = editSeasonDTO.Description;

            // Update the season in the database
            _dfcStatsDbContext.Seasons.Update(existingSeason);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the updated season to a SeasonDTO and return it
            return existingSeason.MapToSeasonDTO()!;
        }
    }
}