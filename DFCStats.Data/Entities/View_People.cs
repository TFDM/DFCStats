using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class View_People
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string LastNameFirstName { get; set; } = string.Empty;
		public string FirstNameLastName { get; set; } = string.Empty;
        public DateTime? DateofBirth { get; set; }
        public Guid? NationalityID { get; set; }
        public string? Nationality { get; set; }
        public string? Icon { get; set; }
        public string? Biography { get; set; }
        public bool IsManager { get; set; }
        public int TotalApps { get; set; }
        public int TotalStartApps { get; set; }
        public int TotalSubApps { get; set; }
        public int TotalGoals { get; set; }
        public decimal? GoalsPerGame { get; set; }
    }
}