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

        /// <summary>
        /// Updates a participation record in the database
        /// </summary>
        /// <param name="updatedParticipationDTO"></param>
        /// <returns></returns>
        Task<ParticipationDTO> UpdateParticipationAsync(ParticipationDTO updatedParticipationDTO);

        /// <summary>
        /// Removes a participation record from the database and reorders the remaining participation records for the fixture
        /// </summary>
        /// <param name="participationDTO"></param>
        /// <returns></returns>
        Task RemoveParticipationAsync(ParticipationDTO participationDTO);

        /// <summary>
        /// Moves a participation record up or down by changing the order number relevant to other participation records
        /// </summary>
        /// <param name="participationDTO"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        Task Move(ParticipationDTO participationDTO, string direction);
    }
}