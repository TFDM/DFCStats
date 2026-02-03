using DFCStats.Business.Interfaces;
using DFCStats.Business.MappingExtensions;
using DFCStats.Data;
using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DFCStats.Business
{
    public class NationalityService : INationalityService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public NationalityService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Gets all the nationalities from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<NationalityDTO>> GetAllNationalitiesAsync()
        {
            // Gets all the nationalities
            var nationalities =  await _dfcStatsDbContext.Nationalities.ToListAsync();

            // Map the nationalities to NationalityDTOs and return them
            return nationalities.Select(n => n.MapToNationalityDTO()!).ToList();
        }

        /// <summary>
        /// Searches for nationalities with optional filtering, sorting, and pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchCountry"></param>
        /// <param name="searchNationality"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<(List<NationalityDTO>, int)> SearchForNationalitiesAsync(int page = 1, int pageSize = 50, string? searchCountry = null, string? searchNationality = null, string? sort = null)
        {
            // Ensure the page and page size are above not zero or negative
            page = (page < 1) ? 1 : page;
            pageSize = (pageSize < 1) ? 50 : pageSize;

            var nationalities = _dfcStatsDbContext.Nationalities.AsQueryable();

            // Filter the records
            if (searchCountry != null)
                nationalities = nationalities.Where(n => n.Country.Contains(searchCountry));

            if (searchNationality != null)
                nationalities = nationalities.Where(n => n.Name.Contains(searchNationality));

            // Sort the records based on the sort parameter
            switch (sort)
			{
				case "nationality_desc":
					nationalities = nationalities.OrderByDescending(n => n.Name);
					break;
				case "country_desc":
					nationalities = nationalities.OrderByDescending(n => n.Country);
					break;
				case "country":
					nationalities = nationalities.OrderBy(n => n.Country);
					break;
				default:
					nationalities = nationalities.OrderBy(n => n.Name);
					break;
			}

            // Counts the total number of records before any pagination is applied
			var totalItemCount = await nationalities.CountAsync();

            // Carries out the query
			var results = await nationalities.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
            
            // Return the nationalities (mapped to NationalityDTO) and the item count
            return (results.Select(n => n.MapToNationalityDTO()!).ToList(), totalItemCount);
        }
    }
}