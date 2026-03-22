using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.DTOs.People;

namespace DFCStats.Domain.DTOs.Seasons
{
    public class SeasonDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set;} = string.Empty;
        public List<PersonDTO>? PeopleAttachedToSeason { get; set; }
        public List<FixtureDTO>? Fixtures { get; set; }
    }
}