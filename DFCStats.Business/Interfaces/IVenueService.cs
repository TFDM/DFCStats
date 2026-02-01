using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface IVenueService
    {
        /// <summary>
        /// Gets all the venues in the database
        /// </summary>
        /// <returns></returns>
        Task<List<VenueDTO>> GetAllVenuesAsync();
    }
    
}