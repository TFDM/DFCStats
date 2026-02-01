using DFCStats.Domain.DTOs;

namespace DFCStats.Business.Interfaces
{
    public interface ISeasonService
    {
        /// <summary>
        /// Adds a season to the database
        /// </summary>
        /// <param name="seasonDTO"></param>
        /// <returns></returns>
        Task<SeasonDTO> AddSeasonAsync(SeasonDTO seasonDTO);
    }
}