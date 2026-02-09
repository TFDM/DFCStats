using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DFCStats.Web.Models.People
{
    public class NewPerson
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;
        [DisplayName("Date of Birth")]
        public DateOnly? DateOfBirth { get; set; }
        [DisplayName("Nationality")]
        public Guid? NationalityId { get; set; }
        public List<Season>? Seasons { get; set; } = new List<Season>();
        public bool IsManager { get; set; }
        public string? Biography { get; set; }
    }

    public class Season
    {
        public Guid SeasonId { get; set; }
        public string? Description { get; set; }
    }
}