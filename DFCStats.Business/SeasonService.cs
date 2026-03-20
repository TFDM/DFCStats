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
        /// Returns a season from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SeasonDTO?> GetSeasonByIdAsync(Guid id, SeasonIncludes includes = SeasonIncludes.None)
        {
            var query = _dfcStatsDbContext.Seasons.AsQueryable();

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
        private async Task<bool> IsSeasonDescriptionInUseAsync(string description)
        {
            return await _dfcStatsDbContext.Seasons.AnyAsync(s => s.Description.ToLower().Trim() == description.ToLower().Trim());
        }

        /// <summary>
        /// Returns a list of all the seasons in the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<List<SeasonDTO>> GetAllSeasonsAsync(string? sort = null)
        {
            // Get all the seasons in the database
            var seasons = _dfcStatsDbContext.Seasons.AsQueryable();

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
    }
}