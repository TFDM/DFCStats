using DFCStats.Domain.DTOs.Messages;

namespace DFCStats.Business.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email used the supplied email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task SendEmailAsync(EmailMessageDTO email);
    }
}