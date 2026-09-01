namespace DFCStats.Domain.DTOs.PasswordResets
{
    public enum PasswordResetTokenStatus
    {
        Valid,
        NotFound,
        Expired,
        AlreadyUsed,
        Invalidated
    }
}