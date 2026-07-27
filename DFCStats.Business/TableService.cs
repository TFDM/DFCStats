using DFCStats.Data;
using DFCStats.Data.Entities;
using DFCStats.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using DFCStats.Domain.DTOs.Tables;
using DFCStats.Domain.Exceptions;
using DFCStats.Business.MappingExtensions;

namespace DFCStats.Business
{
    public class TableService : ITableService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;
        
        public TableService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Adds a new table entry to the database
        /// </summary>
        /// <param name="tableDTO"></param>
        /// <returns></returns>
        public async Task<TableDTO> AddTableEntryAsync(TableDTO tableDTO)
        {
            // Check the flags for the table entry
            CheckFlags(tableDTO);

            // Gets the table for the season specified in the DTO
            var tableForSeason = await GetTableBySeasonIdAsync(tableDTO.SeasonId, TableIncludes.Clubs, sort: "position_desc");

            // Checks that the club is not already in the table for the season
            CheckClubNotAlreadyInTable(tableDTO, tableForSeason);

            // Get the last position in the table for the season and then increase it by 1 to get the position for the new table entry
            var position = tableForSeason.Max(t => t.Position);
            position++;

            return new TableDTO();
        }

        /// <summary>
        /// Gets a table for a specific season
        /// </summary>
        /// <param name="seasonId"></param>
        /// <param name="includes"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<List<TableDTO>> GetTableBySeasonIdAsync(Guid seasonId, TableIncludes includes = TableIncludes.None, string? sort = null)
        {
            // Get the table entries for the specified season
            var tableForSeason = _dfcStatsDbContext.Tables
                .AsNoTracking().AsQueryable().Where(t => t.SeasonId == seasonId);

            // Includes the people attached to the season and then the people themselves
            if (includes.HasFlag(TableIncludes.Clubs))
                tableForSeason = tableForSeason.Include(t => t.Club);

            // Apply sorting if specified
            switch(sort)
            {
                case "position_desc":
                    tableForSeason = tableForSeason.OrderByDescending(t => t.Position);
                    break;
                default:
                    tableForSeason = tableForSeason.OrderBy(t => t.Position);
                    break;
            }

            // Map the table entries to DTOs and return them
            return await tableForSeason.Select(n => n.MapToTableDTO()!).ToListAsync();
        }

        /// <summary>
        /// Checks the is champion, is promotion, is play-offs and is relegated flags
        /// </summary>
        /// <param name="tableDTO"></param>
        /// <exception cref="DFCStatsException"></exception>
        private void CheckFlags(TableDTO tableDTO)
		{
			// Each boolean is treated as 1 if true and 0 if false.
            // The sum gives the number of true values and it should always be 1 or less
			bool onlyOneOrNoneTrue =
				(tableDTO.IsChampion ? 1 : 0) +
				(tableDTO.IsPromotion ? 1 : 0) +
				(tableDTO.IsPlayOff ? 1 : 0) +
				(tableDTO.IsRelegation ? 1 : 0) <= 1;

			if (!onlyOneOrNoneTrue)
                throw new DFCStatsException("Only one of is Champion, is promotion, is play-offs and is relgated can be set to yes");
		}

        /// <summary>
        /// Checks that club is not already in the table for the season
        /// </summary>
        /// <param name="tableDTO"></param>
        /// <param name="currentTable"></param>
        private void CheckClubNotAlreadyInTable(TableDTO tableDTO, List<TableDTO> currentTable)
        {
            // If the club is specified, check that it is not already in the table for the season
            if (tableDTO.ClubId.HasValue && currentTable.Any(t => t.ClubId == tableDTO.ClubId))
                throw new DFCStatsException("The club is already in the table for this season");

            // If the club is not specified, check that there is not already a table entry with a club id of no value for the season
            if (!tableDTO.ClubId.HasValue && currentTable.Any(t => !t.ClubId.HasValue))
                throw new DFCStatsException("Darlington already has a table entry for an unknown club in this season");
        }

    }
}