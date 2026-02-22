namespace DFCStats.Domain.DTOs.People
{
    public class PersonDTO
    {
        public Guid Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastNameFirstName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public Guid? NationalityId { get; set; }
        public string? Nationality { get; set; }
        public string? NationalityIcon { get; set; }
        public string? Biography { get; set; }
        public int TotalApps { get; set; }
        public int TotalGoals { get; set; }
        public decimal GoalsPerGame { get; set; }
        public List<Season>? Seasons { get; set; }
        public bool IsManager { get; set; }
    }

    public class Season
    {
        public Guid Id { get; set; }
        public string Description { get; set;} = string.Empty;
    }

    public class NewPersonDTO
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public Guid? NationalityId { get; set; }
        public string? Biography { get; set; }
        public bool IsManager { get; set; }
        public List<Guid>? ListOfSeasons { get; set; }
    }

    public class EditPersonDTO
    {
        public Guid Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public Guid? NationalityId { get; set; }
        public string? Biography { get; set; }
        public bool IsManager { get; set; }
        public List<Guid>? ListOfSeasons { get; set; }
    }
}