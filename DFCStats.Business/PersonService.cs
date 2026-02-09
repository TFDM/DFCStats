using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs;
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
        /// <returns></returns>
        public async Task<PersonDTO?> GetPersonByIdAsync(Guid id)
        {
            // Get the person from the database
            var person = await _dfcStatsDbContext.People.Include(p => p.Nationality).FirstOrDefaultAsync(p => p.Id == id);

            // If not found, return null
            if (person == null)
                return null;

            // Map the entity to a DTO and return it
            return person.MapToPersonDTO();
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
                var nationality = _nationalities.GetNationalityByIdAsync((Guid)nationalityId);

                //If the nationality wasn't found then throw an error
                if (nationality == null)
                    throw new DFCStatsException("Nationality was not found in the database");
            }
        }



    }
}