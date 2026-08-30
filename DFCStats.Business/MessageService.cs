using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs.Messages;
using DFCStats.Domain.Exceptions;
using HandlebarsDotNet;

namespace DFCStats.Business
{
    public class MessageService : IMessageService
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private const string _emailTemplateName = "Standard Template";

        public MessageService(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        /// <summary>
        /// Builds an email message using supplied place holders and template
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="subject"></param>
        /// <param name="placeHolders"></param>
        /// <param name="recipients"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        public async Task<EmailMessageDTO> BuildEmailAsync(string templateName, string subject, object placeHolders, List<string> recipients)
        {
            // Get the standard template - regardless of which template is requesated all other templates sit inside of this
            var standardTemplate = await _emailTemplateService.GetEmailTemplateByTitleAsync(_emailTemplateName);

            // Get the requested template
            var requestedTemplate = await _emailTemplateService.GetEmailTemplateByTitleAsync(templateName);

            // Check that both the standard template and the requested template have been found
            if (standardTemplate == null || requestedTemplate == null)
                // If either of the templates are null then throw an exception
                throw new DFCStatsException("Unable to find email templates");

            // Compile the requested template with the placeholder values
            var innerBody = Handlebars.Compile(requestedTemplate.Template)(placeHolders);

            // Wrap the compiled inner content inside the standard template
            var outerBody = Handlebars.Compile(standardTemplate.Template)(new { Title = subject, Body = innerBody });

            // Create the email message
            var emailMessage = new EmailMessageDTO
            {
                Body = outerBody,
                Recipients = recipients,
                SubjectLine = subject
            };

            // Return the email message
            return emailMessage;
        }
    }
}