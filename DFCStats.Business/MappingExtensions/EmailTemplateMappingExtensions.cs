using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.EmailTemplates;

namespace DFCStats.Business.MappingExtensions
{
    public static class EmailTemplateMappingExtensions
    {
        /// <summary>
        /// Maps an email template entity to a EmailTemplateDTO
        /// </summary>
        /// <param name="emailTemplate"></param>
        /// <returns></returns>
        public static EmailTemplateDTO? MapToEmailTemplateDTO(this EmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
                return null;

            return new EmailTemplateDTO
            {
                Id = emailTemplate.Id,
                Title = emailTemplate.Title,
                Template = emailTemplate.Template,
                IsHtml = emailTemplate.IsHtml
            };
        }
    }
}