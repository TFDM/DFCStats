using DFCStats.Domain.DTOs.EmailTemplates;

namespace DFCStats.Business.Interfaces
{
    public interface IEmailTemplateService
    {
        /// <summary>
        /// Gets an email template by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EmailTemplateDTO?> GetTemplateByIdAsync(Guid id);

        /// <summary>
        /// Adds a new email templte to the database
        /// </summary>
        /// <param name="newEmailTemplateDTO"></param>
        /// <returns></returns>
        Task<EmailTemplateDTO> AddTemplateAsync(EmailTemplateDTO newEmailTemplateDTO);

        /// <summary>
        /// Updates an exisiting email template
        /// </summary>
        /// <param name="editEmailTemplateDTO"></param>
        /// <returns></returns>
        Task<EmailTemplateDTO> UpdateTemplateAsync(EmailTemplateDTO editEmailTemplateDTO);

        /// <summary>
        /// Gets an email template from the database using the title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<EmailTemplateDTO?> GetEmailTemplateByTitleAsync(string title);

        /// <summary>
        /// Gets all the email templates in the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<EmailTemplateDTO>> GetAllEmailTemplatesAsync(string? sort = null);
    }
}