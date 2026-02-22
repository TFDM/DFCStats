namespace DFCStats.Domain.DTOs.Seasons
{
    public class SeasonDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set;} = string.Empty;
        public List<PersonAttachedToSeason>? PeopleAttachedToSeason { get; set; }
    }

    public class PersonAttachedToSeason
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastNameFirstName { get; set; } = string.Empty;
    }
}