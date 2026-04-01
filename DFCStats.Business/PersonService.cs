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
        private readonly ISeasonService _seasons;
        private readonly INationalityService _nationalities;

        public PersonService(DFCStatsDBContext dFCStatsDBContext, ISeasonService seasons, INationalityService nationalities)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
            _seasons = seasons;
            _nationalities = nationalities;
        }

        /// <summary>
        /// Returns a person from the database using the id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<PersonDTO?> GetPersonByIdAsync(Guid id, PersonIncludes includes = PersonIncludes.None)
        {
            var x = _dfcStatsDbContext.View_People.FirstOrDefault(p => p.Id == id);

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

            return person?.MapToPersonDTO();
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
                var allSeasons = await _seasons.GetAllSeasonsAsync();
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
                var nationality = await _nationalities.GetNationalityByIdAsync((Guid)nationalityId);

                //If the nationality wasn't found then throw an error
                if (nationality == null)
                    throw new DFCStatsException("Nationality was not found in the database");
            }
        }



    }
}