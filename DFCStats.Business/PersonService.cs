using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs.People;
using DFCStats.Domain.DTOs.Fixtures;
using Microsoft.EntityFrameworkCore;
using DFCStats.Business.MappingExtensions;
using DFCStats.Domain.Exceptions;
using DFCStats.Data.Entities;

namespace DFCStats.Business
{
    public class PersonService : IPersonService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;
        private readonly ISeasonService _seasonService;
        private readonly INationalityService _nationalityService;
        private readonly IManagerService _managerService;

        public PersonService(DFCStatsDBContext dFCStatsDBContext, ISeasonService seasonService, INationalityService nationalityService, IManagerService managerService)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
            _seasonService = seasonService;
            _nationalityService = nationalityService;
            _managerService = managerService;
        }

        /// <summary>
        /// Searches for people with optional filtering, sorting, and pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchFirstName"></param>
        /// <param name="searchLastName"></param>
        /// <param name="searchNationalityId"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<(List<PersonDTO>, int)> SearchForPeopleAsync(int page = 1, int pageSize = 50, string? searchFirstName = null, string? searchLastName = null, 
            string? searchNationalityId = null, string? sort = null)
        {
            // Ensure the page and page size are above not zero or negative
            page = (page < 1) ? 1 : page;
            pageSize = (pageSize < 1) ? 50 : pageSize;

            var people = _dfcStatsDbContext.View_People
                .AsNoTracking()
                .AsQueryable();

            // Filter the records
            if (searchFirstName != null)
            {
                people = people.Where(x => x.FirstName.Contains(searchFirstName));
            }
            if (searchLastName != null)
            {
                people = people.Where(x => x.LastName.Contains(searchLastName));
            }
            if (searchNationalityId != null)
            {
                people = people.Where(x => x.NationalityID == Guid.Parse(searchNationalityId));
            }

            // Sort the records
            switch (sort)
            {
                case "lastnamefirstname_desc":
                    people = people.OrderByDescending(x => x.LastNameFirstName);
                    break;
                case "dateofbirth_desc":
                    people = people.OrderByDescending(x => x.DateofBirth);
                    break;
                case "dateofbirth":
                    people = people.OrderBy(x => x.DateofBirth);
                    break;
                case "nationality_desc":
                    people = people.OrderByDescending(x => x.Nationality);
                    break;
                case "nationality":
                    people = people.OrderBy(x => x.Nationality);
                    break;
                case "totalapps_desc":
                    people = people.OrderByDescending(x => x.TotalApps);
                    break;
                case "totalapps":
                    people = people.OrderBy(x => x.TotalApps);
                    break;
                case "totalgoals_desc":
                    people = people.OrderByDescending(x => x.TotalGoals);
                    break;
                case "totalgoals":
                    people = people.OrderBy(x => x.TotalGoals);
                    break;
                case "goalsPerGame_desc":
                    people = people.OrderByDescending(x => x.GoalsPerGame);
                    break;
                case "goalsPerGame":
                    people = people.OrderBy(x => x.GoalsPerGame);
                    break;
                default:
                    people = people.OrderBy(x => x.LastNameFirstName);
                    break;
            }

            // Counts the total number of records before any pagination is applied
			var totalItemCount = await people.CountAsync();

            // Carries out the query
			var results = await people.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            // Return the people (mapped to PeopleDTO) and the item count
            return (results.Select(p => p.MapToPersonDTO()!).ToList(), totalItemCount);
        }

        /// <summary>
        /// Returns a person from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<PersonDTO?> GetPersonByIdAsync(Guid id, PersonIncludes includes = PersonIncludes.None)
        {
            var query = _dfcStatsDbContext.People.AsNoTracking().AsQueryable();
    
            // Includes the nationality
            if (includes.HasFlag(PersonIncludes.Nationality))
                query = query.Include(p => p.Nationality);
        
            // Includes the PersonSeasons - also include the seasons as well so the description is included
            if (includes.HasFlag(PersonIncludes.Seasons))
                query = query.Include(p => p.PersonSeasons).ThenInclude(s => s.Season);

            // Includes the stats - also includes the fixture and the category so the stats can be properly caculated
            if (includes.HasFlag(PersonIncludes.Stats))
                query = query.Include(p => p.Participation).ThenInclude(f => f.Fixture).ThenInclude(c => c!.Category)
                            .Include(p => p.Participation).ThenInclude(f => f.Fixture).ThenInclude(s => s!.Season);

            // Run the query and map the entity to a DTO and return it
            var person = await query.FirstOrDefaultAsync(p => p.Id == id);

            // Map the person to a PersonDTO
            var personDTO = person?.MapToPersonDTO();

            // Includes the management management spell for a person
            if (includes.HasFlag(PersonIncludes.ManagementRecord))
            {
                // Get the management records for the person
                var managementRecords = await _managerService.GetManagementRecordsByPersonIdAsync(id);

                // Add the management records to the personDTO
                personDTO!.ManagementSpells = managementRecords;            
            }

            return personDTO;
        }

        /// <summary>
        /// Gets people who are managers
        /// </summary>
        /// <returns></returns>
        public async Task<List<PersonDTO>> GetPeopleWhoAreManagersAsync(string? sort = null)
        {
            // Gets all the people who are marked as managers in the database
            var peopleWhoAreManagers = _dfcStatsDbContext.People.AsNoTracking()
                .Where(p => p.IsManager)
                .AsQueryable();

            // Sort the records based on the sort parameter
            switch (sort)
            {
                case "lastName_desc":
                    peopleWhoAreManagers = peopleWhoAreManagers.OrderByDescending(p => p.LastName);
                    break;
                case "lastName":
                    peopleWhoAreManagers = peopleWhoAreManagers.OrderBy(p => p.LastName);
                    break;
            }

            // Map the people to PersonDTO and return them
            return await peopleWhoAreManagers.Select(p => p.MapToPersonDTO()!).ToListAsync();
        }

        /// <summary>
        /// Adds a new person to the database
        /// </summary>
        /// <param name="newPersonDTO"></param>
        /// <returns></returns>
        public async Task<PersonDTO> AddPersonAsync(NewPersonDTO newPersonDTO)
        {
            // Check the supplied seasons are in the database and haven't been repeated
            await CheckSeaonsAsync(newPersonDTO.ListOfSeasons);

            // Check the nationality exists in the database
            await CheckNationalityAsync(newPersonDTO.NationalityId);

            var peopleSeasons = new List<DFCStats.Data.Entities.PersonSeason>();

            if (newPersonDTO.ListOfSeasons != null)
            {
                // Create a new list of PersonSeason which can added to the new person
                peopleSeasons = newPersonDTO.ListOfSeasons.Select(seasonId => new PersonSeason
                {
                    Id = Guid.NewGuid(),
                    SeasonId = seasonId
                }).ToList();
            }

            // Create the person using the Dto
            var person = new Person { 
                FirstName = newPersonDTO.FirstName,
                LastName = newPersonDTO.LastName,
                DateOfBirth = newPersonDTO.DateOfBirth,
                NationalityId = newPersonDTO.NationalityId,
                Biography = newPersonDTO.Biography,
                IsManager = newPersonDTO.IsManager,
                PersonSeasons = peopleSeasons 
            };

            await _dfcStatsDbContext.People.AddAsync(person);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created person to a PersonDTO and return it
            return person.MapToPersonDTO()!;
        }

        /// <summary>
        /// Gets the fixtures a selected person appeared in for a selected season
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="seasonId"></param>
        /// <returns></returns>
        public async Task<List<ParticipationFixtureDTO>> GetParticipatedFixturesAsync(Guid personId, Guid seasonId)
        {
            //Gets all the fixtures participated by a person for a given season
            var fixturesParticipatedIn = await _dfcStatsDbContext.Participants
                .Include(p => p.Fixture).ThenInclude(f => f!.Season)
                .Include(p => p.Fixture).ThenInclude(f => f!.Club)
                .Include(p => p.Fixture).ThenInclude(f => f!.Venue)
                .Where(p => p.PersonId == personId && p.Fixture!.SeasonId == seasonId)
                .OrderBy(p => p.Fixture!.Date)
                .ToListAsync();

            // Map the participation records to ParticipationFixtureDTO and return it
            return fixturesParticipatedIn.Select(f => f.MapToParticipationFixtureDTO()!).ToList();
        }

        /// <summary>
        /// Update the person in the database
        /// </summary>
        /// <param name="editPersonDTO"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        public async Task<PersonDTO> UpdatePersonAsync(EditPersonDTO editPersonDTO)
        {
            // Check the supplied seasons are in the database and haven't been repeated
            await CheckSeaonsAsync(editPersonDTO.ListOfSeasons);

            // Check the nationality exists in the database
            await CheckNationalityAsync(editPersonDTO.NationalityId);

            // Get the person from the database
            var existingPerson = await _dfcStatsDbContext.People
                .Include(p => p.PersonSeasons)
                .FirstOrDefaultAsync(p => p.Id == editPersonDTO.Id);

            // Check if the person exists in the database
            if (existingPerson == null)
                throw new DFCStatsException($"Person with id {editPersonDTO.Id} not found");

            // Clear existing seasons
            existingPerson.PersonSeasons.Clear();

            var peopleSeasons = new List<DFCStats.Data.Entities.PersonSeason>();

            if (editPersonDTO.ListOfSeasons != null)
            {
                // Create a new list of PersonSeason which can added to the new person
                peopleSeasons = editPersonDTO.ListOfSeasons.Select(seasonId => new PersonSeason
                {
                    Id = Guid.NewGuid(),
                    SeasonId = seasonId
                }).ToList();
            }

            // Update the person
            existingPerson.FirstName = editPersonDTO.FirstName;
            existingPerson.LastName = editPersonDTO.LastName;
            existingPerson.DateOfBirth = editPersonDTO.DateOfBirth;
            existingPerson.NationalityId = editPersonDTO.NationalityId;
            existingPerson.Biography = editPersonDTO.Biography;
            existingPerson.IsManager = editPersonDTO.IsManager;
            existingPerson.PersonSeasons = peopleSeasons;

            // Update the person in the database
            _dfcStatsDbContext.People.Update(existingPerson);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the updated person to a PersonDTO and return it
            return existingPerson.MapToPersonDTO()!;
        }

        /// <summary>
        /// Checks the supplied list of guids to see if they exist in the seasons table
        /// </summary>
        /// <param name="listOfSeasons"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        private async Task CheckSeaonsAsync(List<Guid>? listOfSeasons)
        {
            // Ensures there are seasons to check
            if (listOfSeasons != null && listOfSeasons.Count > 0)
            {
                // Check for duplicates first (no DB call needed)
                bool hasDuplicates = listOfSeasons.GroupBy(g => g).Any(g => g.Count() > 1);
                if (hasDuplicates)
                    throw new DFCStatsException("A season has been used more than once");

                // Get all seasons from the database
                var allSeasons = await _seasonService.GetAllSeasonsAsync();
                var validSeasonIds = allSeasons.Select(s => s.Id).ToHashSet();
        
                // Check if all provided season ids exist
                var invalidSeasons = listOfSeasons.Where(id => !validSeasonIds.Contains(id)).ToList();
                if (invalidSeasons.Any())
                    throw new DFCStatsException("One or more seasons have not been found in the database");
            }
        }

        /// <summary>
        /// Checks a nationality exists in the database
        /// </summary>
        /// <param name="nationalityId"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        private async Task CheckNationalityAsync(Guid? nationalityId)
        {
            if (nationalityId != null)
            {
                //Get the nationality from the database using the id
                var nationality = await _nationalityService.GetNationalityByIdAsync((Guid)nationalityId);

                //If the nationality wasn't found then throw an error
                if (nationality == null)
                    throw new DFCStatsException("Nationality was not found in the database");
            }
        }



    }
}