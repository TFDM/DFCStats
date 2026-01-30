using DFCStats.Business.Interfaces;
using DFCStats.Data;
using DFCStats.Domain.DTOs;
using DFCStats.Data.Entities;
using Microsoft.EntityFrameworkCore;
using DFCStats.Domain.Exceptions;

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
        /// Check to see if a club name is already in use 
        /// Will return true if it is in use otherwise false
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> IsClubNameInUse(string name)
        {
            return await _dfcStatsDbContext.Clubs.AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        /// <summary>
        /// Gets all the clubs from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ClubDTO>> GetAllClubsAsync()
        {
            // Gets all the clubs
            var clubs = await _dfcStatsDbContext.Clubs.ToListAsync();

            // Convert the clubs to a list of ClubDTO
            return clubs.Select(c => new ClubDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        /// <summary>
        /// Adds a club to the database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AddClub(ClubDTO dto)
        {
            // Check to see if the club name is already in use
            if(await IsClubNameInUse(dto.Name))
            {
                throw new DFCStatsException($"{dto.Name} is already in use" );
            }

            // Create the club using the dto
            var club = new Club() { Name = dto.Name };

            // Add the club to the database and save the changes
            await _dfcStatsDbContext.Clubs.AddAsync(club);
            await _dfcStatsDbContext.SaveChangesAsync();
        }
    }
}