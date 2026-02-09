using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DFCStats.Web.Models.People
{
    public class EditPerson
    {
        public Guid Id { get; set; }
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
}