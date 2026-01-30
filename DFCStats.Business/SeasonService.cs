using DFCStats.Data;
using DFCStats.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DFCStats.Business
{
    public class SeasonService : ISeasonService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public SeasonService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Check to see if a season description is already in use 
        /// Will return true if it is in use otherwise false
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<bool> IsSeasonDescriptionInUse(string description)
        {
            return await _dfcStatsDbContext.Seasons.AnyAsync(s => s.Description.ToLower().Trim() == description.ToLower().Trim());
        }
    }
}