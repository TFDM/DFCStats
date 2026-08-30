namespace DFCStats.Domain.DTOs.Messages
{
    public class EmailMessageDTO
    {
        public List<string> Recipients { get; set; } = new List<string>();
        public string SubjectLine { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

    }
}