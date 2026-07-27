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

            // Set the position for the new table entry to 1
            var position = 1;

            // If there are already table entries for the season, get the last position and increase it by 1
            if (tableForSeason.Count > 0)
            {
                // Get the last position in the table for the season and then increase it by 1 to get the position for the new table entry
                position = tableForSeason.Max(t => t.Position);
                position++;
            }

            // Create the table entry using the dto and the calculated position
            var tableEntry = new Table()
            {
                SeasonId = tableDTO.SeasonId,
                ClubId = tableDTO.ClubId,
                Position = position,
                Played = tableDTO.GamesPlayed,
                HomeWon = tableDTO.HomeGamesWon,
                HomeDrawn = tableDTO.HomeGamesDrawn,
                HomeLost = tableDTO.HomeGamesLost,
                HomeGoalsFor = tableDTO.HomeGoalsFor,
                HomeGoalsAgainst = tableDTO.HomeGoalsAgainst,
                AwayWon = tableDTO.AwayGamesWon,
                AwayDrawn = tableDTO.AwayGamesDrawn,
                AwayLost = tableDTO.AwayGamesLost,
                AwayGoalsFor = tableDTO.AwayGoalsFor,
                AwayGoalsAgainst = tableDTO.AwayGoalsAgainst,
                Points = tableDTO.Points,
                IsChampion = tableDTO.IsChampion,
                IsPromotion = tableDTO.IsPromotion,
                IsPlayOffs = tableDTO.IsPlayOff,
                IsRelegated = tableDTO.IsRelegation,
                IsDarlington = tableDTO.IsDarlington,
                Notes = tableDTO.Notes
            };

            // Add the table entry to the database and save the changes
            await _dfcStatsDbContext.Tables.AddAsync(tableEntry);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created table entry to a TableDTO and return it
            return tableEntry.MapToTableDTO()!;
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
                throw new DFCStatsException("Darlington already has a table entry for this season");
        }

    }
}