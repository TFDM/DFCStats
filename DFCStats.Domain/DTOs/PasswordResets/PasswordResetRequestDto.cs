namespace DFCStats.Domain.DTOs.PasswordResets
{
    public class PasswordResetRequestDTO
    {
        public string EmailAddress { get; set;} = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
    }
}