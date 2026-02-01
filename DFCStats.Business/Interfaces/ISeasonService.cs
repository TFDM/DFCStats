using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface ISeasonService
    {
        /// <summary>
        /// Returns a list of all the seasons in the database
        /// </summary>
        /// <returns></returns>
        Task<List<SeasonDTO>> GetAllSeasonsAsync();

        /// <summary>
        /// Adds a season to the database
        /// </summary>
        /// <param name="seasonDTO"></param>
        /// <returns></returns>
        Task<SeasonDTO> AddSeasonAsync(SeasonDTO seasonDTO);
    }
}