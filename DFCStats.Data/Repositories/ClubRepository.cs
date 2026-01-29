using DFCStats.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DFCStats.Data.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly DFCStatsDBContext _dbContext;

        public ClubRepository(DFCStatsDBContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<List<DFCStats.Domain.Entities.Club>> GetAllAsync()
        {
            return await _dbContext.Clubs.ToListAsync();
        }

        public Task AddAsync(DFCStats.Domain.Entities.Club club)
        {
            _dbContext.Clubs.Add(club);
            return Task.CompletedTask;
        }
    }
}