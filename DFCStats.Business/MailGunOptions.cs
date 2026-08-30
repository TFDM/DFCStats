namespace DFCStats.Business
{
    public class MailgunOptions
    {
        public required string Username { get; set; }
        public required string ApiKey { get; set; }
        public required string Domain { get; set; }
        public required string BaseUrl { get; set; } = "https://api.mailgun.net/v3"; // api.eu.mailgun.net for EU region
        public required string FromAddress { get; set; }
        public string FromName { get; set; } = "";
    }
}