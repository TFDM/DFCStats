namespace DFCStats.Domain.DTOs.Users
{
    public class LoginResultDTO
    {
        public UserDTO? User { get; set; }
        public bool Succeeded { get; init; }
        public LoginFailureReason? FailureReason { get; init; }
    }

    public enum LoginFailureReason
    {
        InvalidCredentials,
        AccountDisabled
    }
}