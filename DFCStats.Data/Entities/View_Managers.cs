using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class View_Managers
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string LastNameFirstName { get; set; } = string.Empty;
        public string FirstNameLastName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public Guid? NationalityId { get; set; }
        public string? Nationality { get; set; }
        public string? Icon { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly AltEndDate { get; set; }
        public bool IsCareTaker { get; set; }
        public int DaysInCharge { get; set; }
        public int GamesManaged { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAg { get; set; }
        public decimal? WinPercentage { get; set; }
    }
}