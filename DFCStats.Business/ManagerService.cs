using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs.Managers;
using Microsoft.EntityFrameworkCore;
using DFCStats.Business.MappingExtensions;
using DFCStats.Data.Entities;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Business
{
    public class ManagerService : IManagerService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public ManagerService(DFCStatsDBContext dfcStatsDbContext)
        {
            _dfcStatsDbContext = dfcStatsDbContext;
        }

        /// <summary>
        /// Returns a list of all the management records paginated. Also allows filtering out of caretaker records
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includeCaretakers"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<(List<ManagementRecordDTO>, int)> GetManagementRecordsWithPaginationAsync(int page = 1, int pageSize = 50, bool includeCaretakers = true, string? sort = null)
        {
            // Ensure the page and page size are above not zero or negative
            page = (page < 1) ? 1 : page;
            pageSize = (pageSize < 1) ? 50 : pageSize;

            var managementRecords = _dfcStatsDbContext.View_Managers.AsNoTracking().AsQueryable();

            // Filter out caretaker records if includeCaretakers is false
            if (!includeCaretakers) 
                managementRecords = managementRecords.Where(m => !m.IsCareTaker);

            // Sorts the records
            switch (sort)
            {
                case "startDate":
                    managementRecords = managementRecords.OrderBy(x => x.StartDate);
                    break;
                case "endDate_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.AltEndDate);
                    break;
                case "endDate":
                    managementRecords = managementRecords.OrderBy(x => x.AltEndDate);
                    break;
                case "managerName_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.LastNameFirstName);
                    break;
                case "managerName":
                    managementRecords = managementRecords.OrderBy(x => x.LastNameFirstName);
                    break;
                case "nationality_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.Nationality);
                    break;
                case "nationality":
                    managementRecords = managementRecords.OrderBy(x => x.Nationality);
                    break;
                case "gamesManaged_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.GamesManaged);
                    break;
                case "gamesManaged":
                    managementRecords = managementRecords.OrderBy(x => x.GamesManaged);
                    break;
                case "won_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.Won);
                    break;
                case "won":
                    managementRecords = managementRecords.OrderBy(x => x.Won);
                    break;
                case "drawn_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.Drawn);
                    break;
                case "drawn":
                    managementRecords = managementRecords.OrderBy(x => x.Drawn);
                    break;
                case "lost_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.Lost);
                    break;
                case "lost":
                    managementRecords = managementRecords.OrderBy(x => x.Lost);
                    break;
                case "goalsFor_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.GoalsFor);
                    break;
                case "goalsFor":
                    managementRecords = managementRecords.OrderBy(x => x.GoalsFor);
                    break;
                case "goalsAgainst_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.GoalsAg);
                    break;
                case "goalsAgainst":
                    managementRecords = managementRecords.OrderBy(x => x.GoalsAg);
                    break;
                case "timeInCharge_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.DaysInCharge);
                    break;
                case "timeInCharge":
                    managementRecords = managementRecords.OrderBy(x => x.DaysInCharge);
                    break;
                case "winsPercentage_desc":
                    managementRecords = managementRecords.OrderByDescending(x => x.WinPercentage);
                    break;
                case "winsPercentage":
                    managementRecords = managementRecords.OrderBy(x => x.WinPercentage);
                    break;
                default:
                    managementRecords = managementRecords.OrderByDescending(x => x.StartDate);
                    break;
            }

            // Counts the total number of records before any pagination is applied
			var totalItemCount = await managementRecords.CountAsync();

            // Carries out the query
            var results = await managementRecords.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            // Return the management records (mapped to MapToManagementRecordDTO) and the item count
            return (results.Select(m => m.MapToManagementRecordDTO()!).ToList(), totalItemCount);
        }

        /// <summary>
        /// Gets the management records for a given person id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<List<ManagementRecordDTO>> GetManagementRecordsByPersonIdAsync(Guid personId)
        {
            var managerRecords = _dfcStatsDbContext.View_Managers.AsNoTracking()
                .Where(m => m.PersonId == personId)
                .AsQueryable();

            // Map the View_Managers to the ManagementRecordDTO and return them
            return await managerRecords.Select(m => m.MapToManagementRecordDTO()!).ToListAsync();
        }
    
        /// <summary>
        /// Gets a management record by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<ManagementRecordDTO?> GetManagementRecordByIdAsync(Guid id, ManagerIncludes includes = ManagerIncludes.None)
        {
            var query = _dfcStatsDbContext.Managers.AsNoTracking().AsQueryable();

            // Include the person if specified
            if (includes.HasFlag(ManagerIncludes.Person))
                query = query.Include(m => m.Person);

            // Include the nationality if specified
            if (includes.HasFlag(ManagerIncludes.Nationality))
                query = query.Include(m => m.Person).ThenInclude(p => p!.Nationality);

            // Run the query and map the entity to a DTO and return it
            var managerRecord = await query.FirstOrDefaultAsync(m => m.Id == id);

            return managerRecord?.MapToManagementRecordDTO();
        }

        /// <summary>
        /// Adds a management record to the database
        /// </summary>
        /// <param name="newManagementRecordDTO"></param>
        /// <returns></returns>
        public async Task<ManagementRecordDTO> AddManagerRecordAsync(ManagementRecordDTO newManagementRecordDTO)
        {
            // Check the dates are valid (start date is before end date, and start date isn't in the future)
            CheckDates(newManagementRecordDTO.StartDate, newManagementRecordDTO.EndDate);

            // Create the management record entity using the dto
            var managementRecord = new Manager
            {
                StartDate = newManagementRecordDTO.StartDate,
                EndDate = newManagementRecordDTO.EndDate,
                PersonId = newManagementRecordDTO.PersonId,
                IsCaretaker = newManagementRecordDTO.IsCaretaker
            };

            // Add the management record to the database
            await _dfcStatsDbContext.Managers.AddAsync(managementRecord);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created management record to a ManagementRecordDTO and return it
            return managementRecord.MapToManagementRecordDTO()!;
        }

        /// <summary>
        /// Updates a management record in the database
        /// </summary>
        /// <param name="editManagerRecordDTO"></param>
        /// <returns></returns>
        public async Task<ManagementRecordDTO> UpdateManagerRecordAsync(ManagementRecordDTO editManagerRecordDTO)
        {
            // Check the date are valid (start date is before end date, and start date isn't in the future)
            CheckDates(editManagerRecordDTO.StartDate, editManagerRecordDTO.EndDate);

            // Get the management record from the database
            var existingManagementRecord = await _dfcStatsDbContext.Managers
                .Include(m => m.Person).ThenInclude(p => p.Nationality)
                .FirstOrDefaultAsync(m => m.Id == editManagerRecordDTO.Id);

            // Check if the management record exists in the database
            if (existingManagementRecord == null)
                throw new DFCStatsException($"Management record with id {editManagerRecordDTO.Id} not found");

            // Update the management record
            existingManagementRecord.PersonId = editManagerRecordDTO.PersonId;
            existingManagementRecord.StartDate = editManagerRecordDTO.StartDate;
            existingManagementRecord.EndDate = editManagerRecordDTO.EndDate;
            existingManagementRecord.IsCaretaker = editManagerRecordDTO.IsCaretaker;

            // Update the management record in the database
            _dfcStatsDbContext.Managers.Update(existingManagementRecord);
            await _dfcStatsDbContext.SaveChangesAsync();

            return existingManagementRecord.MapToManagementRecordDTO()!;
        }

        /// <summary>
        /// Check the dates are valid for the management record
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <exception cref="DFCStatsException"></exception>
        private void CheckDates(DateOnly startDate, DateOnly? endDate)
        {
            // Check that the start date isn't in the future
            if (startDate > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new DFCStatsException("Start date cannot be in the future");

            // Check that the end date is not null for the remaining checks
            if (endDate != null)
            {
                // Check that the end date is not before the start date
                if (endDate < startDate)
                    throw new DFCStatsException("End date cannot be before start date");

                // Check that the end date isn't in the future
                if (endDate > DateOnly.FromDateTime(DateTime.UtcNow))
                    throw new DFCStatsException("End date cannot be in the future");
            }
        }
    }
}