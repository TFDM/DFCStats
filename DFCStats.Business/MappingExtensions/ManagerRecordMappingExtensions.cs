using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Managers;
using DFCStats.Business.Helpers;

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
                LastName = managerRecord.LastName,
                FirstName = managerRecord.FirstName,
                LastNameFirstName = managerRecord.LastNameFirstName,
                FirstNameLastName = managerRecord.FirstNameLastName,
                Nationality = managerRecord.Nationality,
                NationalityIcon = managerRecord.Icon,
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
                DaysInCharge = managerRecord.DaysInCharge,
                TimeInChargeAsString = DateAndTimeHelper.TimeAsString(managerRecord.StartDate, managerRecord.AltEndDate)
            };
        }

        /// <summary>
        /// Maps a manager entity to a ManagementRecordDTO
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static ManagementRecordDTO? MapToManagementRecordDTO(this Manager manager)
        {
            if (manager == null)
                return null;

            return new ManagementRecordDTO
            {
                Id = manager.Id,
                LastName = manager.Person?.LastName,
                FirstName = manager.Person?.FirstName,
                LastNameFirstName = $"{manager.Person?.LastName}, {manager.Person?.FirstName}",
                FirstNameLastName = $"{manager.Person?.FirstName} {manager.Person?.LastName}",
                Nationality = manager.Person?.Nationality?.Name,
                NationalityIcon = manager.Person?.Nationality?.Icon,
                PersonId = manager.PersonId,
                StartDate = manager.StartDate,
                EndDate = manager.EndDate,
                IsCaretaker = manager.IsCaretaker
            };
        }
    }
}