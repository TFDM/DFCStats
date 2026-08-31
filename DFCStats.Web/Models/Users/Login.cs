using System.ComponentModel;

namespace DFCStats.Web.Models.Users
{
    public class Login
    {
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}