using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Managers;

namespace DFCStats.Business.MappingExtensions
{
    public static class ManagerRecordMappingExtensions
    {
        /// <summary>
        /// Maps a View_Managers entity to a ManagementRecordDTO
        /// </summary>
        /// <param name="managerRecord"></param>
        /// <returns></returns>
        public static ManagementRecordDTO? MapToManagementRecordDTO(this View_Managers managerRecord)
        {
            if (managerRecord == null)
                return null;

            return new ManagementRecordDTO
            {
                Id = managerRecord.Id,
                PersonId = managerRecord.PersonId,
                StartDate = managerRecord.StartDate,
                EndDate = managerRecord.EndDate,
                NumberOfGamesManaged = managerRecord.GamesManaged,
                IsCaretaker = managerRecord.IsCareTaker,
                Wins = managerRecord.Won,
                Draws = managerRecord.Drawn,
                Losses = managerRecord.Lost,
                GoalsFor = managerRecord.GoalsFor,
                GoalsAgainst = managerRecord.GoalsAg,
                WinPercentage = managerRecord.WinPercentage,
                DaysInCharge = managerRecord.DaysInCharge
            };
        }
    }
}