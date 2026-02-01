using DFCStats.Data;
using DFCStats.Data.Entities;
using DFCStats.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using DFCStats.Domain.DTOs;
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
        /// <returns></returns>
        public async Task<List<SeasonDTO>> GetAllSeasonsAsync()
        {
            // Get all the seasons in the database
            var seasons = await _dfcStatsDbContext.Seasons.ToListAsync();

            // Map the seasons to SeasonDTOs and return them
            return seasons.Select(s => s.MapToSeasonDTO()!).ToList();
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