using DFCStats.Data;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs.EmailTemplates;
using Microsoft.EntityFrameworkCore;
using DFCStats.Business.MappingExtensions;
using DFCStats.Domain.Exceptions;
using DFCStats.Data.Entities;

namespace DFCStats.Business
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;

        public EmailTemplateService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        /// <summary>
        /// Gets an email template by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EmailTemplateDTO?> GetTemplateByIdAsync(Guid id)
        {
            var template = await _dfcStatsDbContext.EmailTemplates.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            // Map the entity to an email template DTO and return it
            return template?.MapToEmailTemplateDTO();
        }

        /// <summary>
        /// Adds a new email templte to the database
        /// </summary>
        /// <param name="newEmailTemplateDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EmailTemplateDTO> AddTemplateAsync(EmailTemplateDTO newEmailTemplateDTO)
        {
            // Get an email template using the title of the new email template
            var emailTemplateWithSameTitle = await GetEmailTemplateByTitleAsync(newEmailTemplateDTO.Title);

            // Check if the email template is null
            if (emailTemplateWithSameTitle != null)
                // If the email template is not null then the title has already been used
                throw new DFCStatsException($"An eamil template with a title of {newEmailTemplateDTO.Title} already exists");

            // Create the email template with the dto
            var newEmailTemplate = new EmailTemplate()
            {
                Title = newEmailTemplateDTO.Title,
                Template = newEmailTemplateDTO.Template,
                IsHtml = newEmailTemplateDTO.IsHtml
            };

            // Save the changes to the database
            await _dfcStatsDbContext.EmailTemplates.AddAsync(newEmailTemplate);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created email emplate to an emailTemplateDTO and return it
            return newEmailTemplate.MapToEmailTemplateDTO()!;
        }

        /// <summary>
        /// Updates an exisiting email template
        /// </summary>
        /// <param name="editEmailTemplateDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EmailTemplateDTO> UpdateTemplateAsync(EmailTemplateDTO editEmailTemplateDTO)
        {
            // Get the email template from the database
            var exisitingEmailTemplate = await _dfcStatsDbContext.EmailTemplates
                .FirstOrDefaultAsync(t => t.Id == editEmailTemplateDTO.Id);

            // Check if the email template exists in the database
            if (exisitingEmailTemplate == null)
                throw new DFCStatsException($"Email template with id {editEmailTemplateDTO.Id} not found");

            // Get an email template using the title of the new email template
            var emailTemplateWithSameTitle = await GetEmailTemplateByTitleAsync(editEmailTemplateDTO.Title);

            // If an email template with the same title was found, and their id's 
            // don't match then this means the title has been used by a different 
            // email template
            if (emailTemplateWithSameTitle != null && emailTemplateWithSameTitle.Id != editEmailTemplateDTO.Id)
                // Throw an exception saying the title can't be used
                throw new DFCStatsException($"An eamil template with a title of {editEmailTemplateDTO.Title} already exists");

            // Update the email template
            exisitingEmailTemplate.Title = editEmailTemplateDTO.Title;
            exisitingEmailTemplate.Template = editEmailTemplateDTO.Template;
            exisitingEmailTemplate.IsHtml = editEmailTemplateDTO.IsHtml;

            // Save the changes to the database
            _dfcStatsDbContext.EmailTemplates.Update(exisitingEmailTemplate);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the updated email template to an EmailTemplateDTO and return it
            return exisitingEmailTemplate.MapToEmailTemplateDTO()!;
        }

        /// <summary>
        /// Gets an email template from the database using the title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<EmailTemplateDTO?> GetEmailTemplateByTitleAsync(string title)
        {
            var query = _dfcStatsDbContext.EmailTemplates.AsNoTracking().AsQueryable();

            // Run the query to find the template by the specified title
            var emailTemplate = await query.FirstOrDefaultAsync(t => t.Title.ToLower() == title.ToLower());

            if (emailTemplate == null)
                return null;

            // Map the entity to a DTO and return it
            return emailTemplate.MapToEmailTemplateDTO();
        }
    
        /// <summary>
        /// Gets all the email templates in the database
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<List<EmailTemplateDTO>> GetAllEmailTemplatesAsync(string? sort = null)
        {
            var templates = _dfcStatsDbContext.EmailTemplates.AsNoTracking().AsQueryable();

            // Sort the records based on the sort parameter
            switch (sort)
            {
                case "title_desc":
                    templates = templates.OrderByDescending(t => t.Title);
                    break;
                default:
                    templates = templates.OrderBy(t => t.Title);
                    break;
            }

            // Map the templates to EmailTemplatesDTO and return them
            return await templates.Select(t => t.MapToEmailTemplateDTO()!).ToListAsync();
        }
    }
}