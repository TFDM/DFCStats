namespace DFCStats.Domain.DTOs.EmailTemplates
{
    public class EmailTemplateDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set;} = string.Empty;
        public string Template { get; set; } = string.Empty;
        public bool IsHtml { get; set; }
    }
}