using DFCStats.Business.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Business
{
    public class MailgunEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly MailgunOptions _options;
        private readonly ILogger<MailgunEmailService> _logger;

        public MailgunEmailService(HttpClient httpClient, IOptions<MailgunOptions> options, ILogger<MailgunEmailService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendSomething()
        {
            // Set up the authentication header
			var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _options.Username, _options.ApiKey)));
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);

            // Prepare form-data
            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent("<p>Hello World</p><p>This is my email.</p>"), "html");
            formData.Add(new StringContent(_options.FromAddress), "from");
            formData.Add(new StringContent("kevin.luff@the-tinshed.co.uk"), "to");
            formData.Add(new StringContent("A test email"), "subject");

            // Set the request uri - this is the base url / domain / messages
            var requestUri = $"{_options.BaseUrl}/{_options.Domain}/messages";

            // Set the request to use the specified form data
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = formData
            };

            // Make the request which sends the email
            var response = await _httpClient.SendAsync(request);

            // Check the response - if its anything other than an error throw an exception
            if (!response.IsSuccessStatusCode)
            {
                // Get the error body
                var errorBody = await response.Content.ReadAsStringAsync();

                // Throw the exception
                throw new DFCStatsException($"Error sending email. Response code: {response.StatusCode}. Error: {errorBody}");
            }

        }
    }


}

