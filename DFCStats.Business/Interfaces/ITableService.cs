using DFCStats.Domain.DTOs.Tables;

namespace DFCStats.Business.Interfaces
{
    [Flags]
    public enum TableIncludes
    {
        // If new flags are required double the previous number
        None = 0,
        Clubs = 1,
        All = Clubs
    }

    public interface ITableService
    {
        /// <summary>
        /// Adds a table entry to the database
        /// </summary>
        /// <param name="tableDTO"></param>
        /// <returns></returns>
        Task<TableDTO> AddTableEntryAsync(TableDTO tableDTO);

        /// <summary>
        /// Gets a table for a specific season
        /// </summary>
        /// <param name="seasonId"></param>
        /// <param name="includes"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<TableDTO>> GetTableBySeasonIdAsync(Guid seasonId, TableIncludes includes = TableIncludes.None, string? sort = null);
    }
}