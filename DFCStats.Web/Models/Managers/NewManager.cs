using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFCStats.Web.Models.Managers
{
    public class NewManager
    {
        [DisplayName("Person")]
        public Guid PersonId { get; set; }
        [DisplayName("Start Date")]
        public DateOnly? StartDate { get; set; }
        [DisplayName("End Date")]
        public DateOnly? EndDate { get; set; }
        [DisplayName("Caretaker")]
        public bool? IsCaretaker { get; set; }

        // Holds the list of options
        public IEnumerable<SelectListItem> ManagerOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> CaretakerOptions { get; set; } = new List<SelectListItem>();
    }
}