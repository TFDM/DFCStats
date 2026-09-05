using DFCStats.Domain.DTOs.PasswordResets;

namespace DFCStats.Business.Interfaces
{
    public interface IPasswordResetService
    {
        int RequestExpiryTimeInMinutes { get; }

        /// <summary>
        /// Issues a password reset email to the user
        /// </summary>
        /// <param name="passwordResetRequestDTO"></param>
        /// <returns></returns>
        Task<bool> RequestPasswordResetAsync(PasswordResetRequestDTO passwordResetRequestDTO);

        /// <summary>
        /// Validates a reset token and ensures it is still valid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PasswordResetTokenStatus> ValidateResetTokenAsync(string token);

        /// <summary>
        /// Rsets the user's password using the provided token and new password
        /// </summary>
        /// <param name="passwordResetDTO"></param>
        /// <returns></returns>
        Task<PasswordResetTokenStatus> ResetPasswordAsync(PasswordResetDTO passwordResetDTO);

        /// <summary>
        /// Deletes password reset requests that have expired more than 
        /// 30 days ago to keep the database clean and prevent unnecessary data accumulation
        /// </summary>
        /// <returns></returns>
        Task DeleteStalePasswordResetRequestsAsync();
    }
}