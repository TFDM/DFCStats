namespace DFCStats.Web.Models.EmailTemplates
{
    public class EmailTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public bool IsHtml { get; set;}
    }
}