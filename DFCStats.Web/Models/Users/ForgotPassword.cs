using System.ComponentModel;

namespace DFCStats.Web.Models.Users
{
    public class ForgotPassword
    {
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}