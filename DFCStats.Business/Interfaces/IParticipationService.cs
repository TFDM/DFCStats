using DFCStats.Domain.DTOs.Participants;

namespace DFCStats.Business.Interfaces
{
    public interface IParticipationService
    {
        Task<ParticipationDTO?> GetParticipationByIdAsync(Guid id);
    }
}