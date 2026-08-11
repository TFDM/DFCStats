using DFCStats.Business.Interfaces;
using DFCStats.Data;
using DFCStats.Domain.DTOs.Clubs;
using DFCStats.Data.Entities;
using Microsoft.EntityFrameworkCore;
using DFCStats.Domain.Exceptions;
using DFCStats.Business.MappingExtensions;

namespace DFCStats.Business
{
    public class ClubService : IClubService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public ClubService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Gets a club by its id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ClubDTO?> GetClubByIdAsync(Guid id)
        {
            // Get the club from the database
            var club = await _dfcStatsDbContext.Clubs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            // If not found, return null
            if (club == null)
                return null;

            // Map the entity to a DTO and return it
            return club.MapToClubDTO();
        }

        /// <summary>
        /// Check to see if a club name is already in use 
        /// Will return true if it is in use otherwise false
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<bool> IsClubNameInUseAsync(string name)
        {
            return await _dfcStatsDbContext.Clubs.AsNoTracking().AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        /// <summary>
        /// Gets all the clubs from the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<List<ClubDTO>> GetAllClubsAsync(string? sort = null)
        {
            // Gets all the clubs
            var clubs = _dfcStatsDbContext.Clubs.AsNoTracking().AsQueryable();

            // Sort the records based on the sort parameter
            switch (sort)
            {
                case "name_desc":
                    clubs = clubs.OrderByDescending(c => c.Name);
                    break;
                case "name":
                    clubs = clubs.OrderBy(c => c.Name);
                    break;
            }

            // Map the clubs to ClubDTOs and return them
            return await clubs.Select(c => c.MapToClubDTO()!).ToListAsync();
        }

        /// <summary>
        /// Returns a list of all the club records paginated. Also allows filtering out of caretaker records
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchName"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<(List<ClubDTO>, int)> SearchForClubsAsync(int page = 1, int pageSize = 50, string? searchName = null, string? sort = null)
        {
            // Ensure the page and page size are above not zero or negative
            page = (page < 1) ? 1 : page;
            pageSize = (pageSize < 1) ? 50 : pageSize;

            var clubs = _dfcStatsDbContext.View_Clubs.AsNoTracking().AsQueryable();

            // Filter the records
            if (searchName != null)
            {
                clubs = clubs.Where(c => c.Name.Contains(searchName));
            }

            // Sort the records
            switch (sort)
            {
                case "name_desc":
                    clubs = clubs.OrderByDescending(c => c.Name);
                    break;
                case "played_desc":
                    clubs = clubs.OrderByDescending(c => c.Played);
                    break;
                case "played":
                    clubs = clubs.OrderBy(c => c.Played);
                    break;
                case "won_desc":
                    clubs = clubs.OrderByDescending(c => c.Won);
                    break;
                case "won":
                    clubs = clubs.OrderBy(c => c.Won);
                    break;
                case "drawn_desc":
                    clubs = clubs.OrderByDescending(c => c.Drawn);
                    break;
                case "drawn":
                    clubs = clubs.OrderBy(c => c.Drawn);
                    break;
                case "lost_desc":
                    clubs = clubs.OrderByDescending(c => c.Lost);
                    break;
                case "lost":
                    clubs = clubs.OrderBy(c => c.Lost);
                    break;
                case "goalsFor_desc":
                    clubs = clubs.OrderByDescending(c => c.GoalsFor);
                    break;
                case "goalsFor":
                    clubs = clubs.OrderBy(c => c.GoalsFor);
                    break;
                case "goalsAgainst_desc":
                    clubs = clubs.OrderByDescending(c => c.GoalsAgainst);
                    break;
                case "goalsAgainst":
                    clubs = clubs.OrderBy(c => c.GoalsAgainst);
                    break;
                default:
                    clubs = clubs.OrderBy(c => c.Name);
                    break;
            }

            // Counts the total number of records before any pagination is applied
			var totalItemCount = await clubs.CountAsync();

            // Carries out the query
			var results = await clubs.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            // Return the clubs (mapped to ClubsDTO) and the item count
            return (results.Select(p => p.MapToClubDTO()!).ToList(), totalItemCount);
        }

        /// <summary>
        /// Adds a club to the database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ClubDTO> AddClubAsync(ClubDTO clubDTO)
        {
            // Check to see if the club name is already in use
            if(await IsClubNameInUseAsync(clubDTO.Name))
                throw new DFCStatsException($"{clubDTO.Name} is already in use" );

            // Create the club using the dto
            var club = new Club() { Name = clubDTO.Name };

            // Add the club to the database and save the changes
            await _dfcStatsDbContext.Clubs.AddAsync(club);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created club to a ClubDTO and return it
            return club.MapToClubDTO()!;
        }
    }
}