using DFCStats.Domain.DTOs.Venues;

namespace DFCStats.Business.Interfaces
{
    public interface IVenueService
    {
        /// <summary>
        /// Gets a venue by its Id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<VenueDTO?> GetVenueByIdAsync(Guid id);

        /// <summary>
        /// Gets all the venues in the database
        /// </summary>
        /// <returns></returns>
        Task<List<VenueDTO>> GetAllVenuesAsync(string? sort = null);
    }
    
}