using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFCStats.Web.Models.Users
{
    public class EditUser
    {
        public Guid Id { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; } = string.Empty;
        public List<RoleCheckBox> Roles { get; set; } = new List<RoleCheckBox>();
        [DisplayName("Allow Login?")]
        public bool? AllowLogin { get; set; }

        // Holds the list of options
        public IEnumerable<SelectListItem> AllowLoginOptions { get; set; } = new List<SelectListItem>();
    }
}