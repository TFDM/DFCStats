using System.ComponentModel;

namespace DFCStats.Web.Models.PasswordResets
{
    public class ForgotPassword
    {
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}