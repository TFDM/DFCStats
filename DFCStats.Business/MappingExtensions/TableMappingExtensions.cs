using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Tables;

namespace DFCStats.Business.MappingExtensions
{
    public static class TableMappingExtensions
    {
        /// <summary>
        /// Maps a Table entity to a TableDTO
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static TableDTO? MapToTableDTO(this Table table)
        {
            if (table == null)
                return null;

            return new TableDTO
            {
                Id = table.Id,
                SeasonId = table.SeasonId,
                ClubId = table.ClubId,
                ClubName = (table.ClubId == null) ? "Darlington" : table.Club?.Name!,
                Position = table.Position,
                GamesPlayed = table.Played,
                HomeGamesWon = table.HomeWon,
                HomeGamesDrawn = table.HomeDrawn,
                HomeGamesLost = table.HomeLost,
                HomeGoalsFor = table.HomeGoalsFor,
                HomeGoalsAgainst = table.HomeGoalsAgainst,
                AwayGamesWon = table.AwayWon,
                AwayGamesDrawn = table.AwayDrawn,
                AwayGamesLost = table.AwayLost,
                AwayGoalsFor = table.AwayGoalsFor,
                AwayGoalsAgainst = table.AwayGoalsAgainst,
                GoalDifference = CalculateGoalDifference(table.HomeGoalsFor, table.HomeGoalsAgainst, table.AwayGoalsFor, table.AwayGoalsAgainst),
                Points = table.Points,
                IsChampion = table.IsChampion,
                IsPromotion = table.IsPromotion,
                IsRelegation = table.IsRelegated,
                IsPlayOff = table.IsPlayOffs,
                Notes = table.Notes
            };
        }

        /// <summary>
        /// Calculate the goal difference based on the goals scored and conceded
        /// </summary>
        /// <param name="homeGoalsFor"></param>
        /// <param name="homeGoalsAgainst"></param>
        /// <param name="awayGoalsFor"></param>
        /// <param name="awayGoalsAgainst"></param>
        /// <returns></returns>
        private static int CalculateGoalDifference(int homeGoalsFor, int homeGoalsAgainst, int awayGoalsFor, int awayGoalsAgainst)
        {
            // Calculate the total goals for and against
            var totalGoalsFor = homeGoalsFor + awayGoalsFor;
            var totalGoalsAgainst = homeGoalsAgainst + awayGoalsAgainst;

            // Return the goal difference
            return totalGoalsFor - totalGoalsAgainst;
        }
    }
}