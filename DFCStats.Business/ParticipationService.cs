using DFCStats.Data;
using DFCStats.Domain.DTOs.Participants;
using Microsoft.EntityFrameworkCore;
using DFCStats.Business.MappingExtensions;
using DFCStats.Business.Interfaces;

namespace DFCStats.Business
{
    public class ParticipationService : IParticipationService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public ParticipationService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        public async Task<ParticipationDTO?> GetParticipationByIdAsync(Guid id)
        {
            var query = _dfcStatsDbContext.Participants.AsNoTracking().AsQueryable();

            query = query.Include(p => p.Fixture).ThenInclude(f => f!.Season)
                .Include(p => p.Fixture).ThenInclude(f => f!.Club)
                .Include(p => p.Fixture).ThenInclude(f => f!.Venue)
                .Include(p => p.Person);

            var participation = await query.FirstOrDefaultAsync(p => p.Id == id);
            return participation?.MapToParticipantsDTO();
        } 
    }
}