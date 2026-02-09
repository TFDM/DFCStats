using DFCStats.Business.Interfaces;
using DFCStats.Business.MappingExtensions;
using DFCStats.Data;
using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs;
using DFCStats.Domain.Exceptions;
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
        /// Returns a nationality from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        public async Task<NationalityDTO?> GetNationalityByIdAsync(Guid id)
        {
            // Get the nationality from the database
            var nationality = await _dfcStatsDbContext.Nationalities.FirstOrDefaultAsync(n => n.Id == id);

            // If not found, return null
            if (nationality == null)
                return null;

            // Map the entity to a DTO and return it
            return nationality.MapToNationalityDTO();
        }

        /// <summary>
        /// Returns a nationality from the database using the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<NationalityDTO?> GetNationalityByNameAsync(string name)
        {
            // Get the nationality from the database using the name
            var nationality = await _dfcStatsDbContext.Nationalities.FirstOrDefaultAsync(n => n.Name.ToLower().Trim() == name.ToLower().Trim());

            // If not found, return null
            if (nationality == null)
                return null;

            // Map the entity to a DTO and return it
            return nationality.MapToNationalityDTO();
        }

        /// <summary>
        /// Gets all the nationalities from the database with optional sorting
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<List<NationalityDTO>> GetAllNationalitiesAsync(string? sort = null)
        {
            // Gets all the nationalities
            var nationalities =  _dfcStatsDbContext.Nationalities.AsQueryable();

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

            // Map the nationalities to NationalityDTOs and return them
            return await nationalities.Select(n => n.MapToNationalityDTO()!).ToListAsync();
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

        /// <summary>
        /// Check to see if a club name is already in use 
        /// Will return true if it is in use otherwise false
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<bool> IsNationalityNameInUseAsync(string nationality)
        {
            return await _dfcStatsDbContext.Nationalities.AnyAsync(n => n.Name.ToLower().Trim() == nationality.ToLower().Trim());
        }

        /// <summary>
        /// Adds a nationality to the database
        /// </summary>
        /// <param name="nationalityDTO"></param>
        /// <returns></returns>
        public async Task<NationalityDTO> AddNationalityAsync(NewNationalityDTO newNationalityDTO)
        {
            // Check to see if the nationality name is already in use
            if(await IsNationalityNameInUseAsync(newNationalityDTO.Nationality))
                throw new DFCStatsException($"{newNationalityDTO.Nationality} is already in use" );

            // Create the nationality using the dto
            var nationality = new Nationality() { Name = newNationalityDTO.Nationality, Country = newNationalityDTO.Country, Icon = newNationalityDTO.Icon };

            await _dfcStatsDbContext.Nationalities.AddAsync(nationality);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created nationality to a NationalityDTO and return it
            return nationality.MapToNationalityDTO()!;
        }

        /// <summary>
        /// Updates a nationality in the database
        /// </summary>
        /// <param name="nationalityDTO"></param>
        /// <returns></returns>
        public async Task<NationalityDTO> UpdateNationalityAsync(EditNationalityDTO editNationalityDTO)
        {
            // Get any nationality with the same name as the one we are trying to update to check if the name is already in use
            var existingNationalityWithName = await GetNationalityByNameAsync(editNationalityDTO.Nationality);

            // If the name is already in use and it's not the same record as the one we are trying to update then throw an exception as the name is already in use
            if (existingNationalityWithName != null && existingNationalityWithName.Id != editNationalityDTO.Id)
                 throw new DFCStatsException($"{editNationalityDTO.Nationality} is already in use" );
            
            // Find the existing nationality in the database
            var existingNationality = await _dfcStatsDbContext.Nationalities.FirstOrDefaultAsync(n => n.Id == editNationalityDTO.Id);

            // Check if the nationality exists in the database
            if (existingNationality == null)
                throw new DFCStatsException($"Nationality with id {editNationalityDTO.Id} not found");

            // Update the existing nationality with the new values
            existingNationality.Name = editNationalityDTO.Nationality;
            existingNationality.Country = editNationalityDTO.Country;
            existingNationality.Icon = editNationalityDTO.Icon;

            // Update the nationality in the database
            _dfcStatsDbContext.Nationalities.Update(existingNationality);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Return the nationalityDTO
            return existingNationality.MapToNationalityDTO()!;
        }
    }
}