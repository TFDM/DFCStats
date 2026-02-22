using DFCStats.Data;
using DFCStats.Domain.DTOs.Venues;
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
        public async Task<List<VenueDTO>> GetAllVenuesAsync(string? sort = null)
        {
            // Get all the venues in the database
            var venues = _dfcStatsDbContext.Venues.AsQueryable();

            // Sort the records based on the sort parameter
            switch (sort)
            {
                case "description_desc":
                    venues = venues.OrderByDescending(v => v.Description);
                    break;
                case "shortDescription_desc":
                    venues = venues.OrderByDescending(v => v.ShortDescription);
                    break;
                case "shortDescription":
                    venues = venues.OrderBy(v => v.ShortDescription);
                    break;
                case "orderNo_desc":
                    venues = venues.OrderByDescending(v => v.OrderNo);
                    break;
                case "orderNo":
                    venues = venues.OrderBy(v => v.OrderNo);
                    break;
                default:
                    venues = venues.OrderBy(v => v.Description);
                    break;
            }

            // Map the seasons to SeasonDTOs and return them
            return await venues.Select(v => v.MapToVenueDTO()!).ToListAsync();
        }
    }
}