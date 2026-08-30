using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFCStats.Web.Models.EmailTemplates
{
    public class EditEmailTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        [DisplayName("Is HTML?")]
        public bool? IsHtml { get; set;}

        // Holds the list of options
        public IEnumerable<SelectListItem> IsHtmlOptions { get; set; } = new List<SelectListItem>();
    }
}