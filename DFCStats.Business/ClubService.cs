using DFCStats.Business.Interfaces;
using DFCStats.Data;
using DFCStats.Domain.DTOs;
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
        /// Check to see if a club name is already in use 
        /// Will return true if it is in use otherwise false
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<bool> IsClubNameInUseAsync(string name)
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

            // Map the clubs to ClubDTOs and return them
            return clubs.Select(c => c.MapToClubDTO()!).ToList();
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