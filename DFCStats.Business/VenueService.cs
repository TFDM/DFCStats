using DFCStats.Data;
using DFCStats.Domain.DTOs;
using DFCStats.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using DFCStats.Business.MappingExtensions;

namespace DFCStats.Business
{
    public class VenueService : IVenueService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public VenueService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Gets all the venues in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<VenueDTO>> GetAllVenuesAsync()
        {
            // Get all the venues in the database
            var venues = await _dfcStatsDbContext.Venues.ToListAsync();

            // Map the seasons to SeasonDTOs and return them
            return venues.Select(v => v.MapToVenueDTO()!).ToList();
        }
    }
}