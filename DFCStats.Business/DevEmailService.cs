using DFCStats.Business.Interfaces;
using Microsoft.Extensions.Logging;
using DFCStats.Domain.DTOs.Messages;

namespace DFCStats.Business
{
    public class DevEmailService : IEmailService
    {
        private readonly ILogger<DevEmailService> _logger;

        public DevEmailService(ILogger<DevEmailService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Uses the email and logs out the details rather than sending an actual email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(EmailMessageDTO email)
        {
            // Logs the email information
            _logger.LogInformation(
                "=== EMAIL (not sent - Development mode) ===\n" +
                "To: {Recipients}\n" +
                "Subject: {Subject}\n" +
                "Body:\n{Body}\n" +
                "=============================================",
                string.Join(", ", email.Recipients),
                email.SubjectLine,
                email.Body);
        }
    }
}