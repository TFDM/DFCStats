using DFCStats.Domain.DTOs.Participants;

namespace DFCStats.Business.Interfaces
{
    public interface IParticipationService
    {
        /// <summary>
        /// Gets a participation by id, including related fixture, season and person details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ParticipationDTO?> GetParticipationByIdAsync(Guid id);

        /// <summary>
        /// Adds a participation record to the database
        /// </summary>
        /// <param name="newParticipationDTO"></param>
        /// <returns></returns>
        Task<ParticipationDTO> AddParticipationAsync(ParticipationDTO newParticipationDTO);
    }
}