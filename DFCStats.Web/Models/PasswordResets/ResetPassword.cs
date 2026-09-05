using System.ComponentModel;

namespace DFCStats.Web.Models.PasswordResets
{
    public class ResetPassword
    {
        public string Token { get; set; } = string.Empty;
        [DisplayName("New Password")]
        public string NewPassword { get; set; } = string.Empty;
        [DisplayName("Confirm New Password")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}