namespace DFCStats.Domain.DTOs
{
    public class PersonDTO
    {
        public Guid Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastNameFirstName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public Guid? NationalityID { get; set; }
        public string? Nationality { get; set; }
        public string? NationalityIcon { get; set; }
        public int TotalApps { get; set; }
        public int TotalGoals { get; set; }
        public decimal GoalsPerGame { get; set; }
    }
}