using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using DFCStats.Business.MappingExtensions;

namespace DFCStats.Business
{
    public class PersonService : IPersonService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public PersonService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
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

    }
}