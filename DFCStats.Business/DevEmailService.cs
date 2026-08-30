using DFCStats.Business.Interfaces;
using Microsoft.Extensions.Logging;

namespace DFCStats.Business
{
    public class DevEmailService : IEmailService
    {
        private readonly ILogger<DevEmailService> _logger;

        public DevEmailService(ILogger<DevEmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendSomething()
        {
            var x = "";
            _logger.LogInformation("Just log stuff instead of sending actual emails");
        }
    }
}