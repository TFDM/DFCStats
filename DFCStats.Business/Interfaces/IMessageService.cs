using DFCStats.Domain.DTOs.Messages;

namespace DFCStats.Business.Interfaces
{
    public interface IMessageService
    {
        /// <summary>
        /// Builds an email message using supplied place holders and template
        /// </summary>
        /// <param name="template"></param>
        /// <param name="subject"></param>
        /// <param name="placeHolders"></param>
        /// <param name="recipients"></param>
        /// <returns></returns>
        Task<EmailMessageDTO> BuildEmailAsync(string template, string subject, object placeHolders, List<string> recipients);
    }
}