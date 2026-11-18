using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Test_Taste_Console_Application.Constants;

namespace Test_Taste_Console_Application.Domain.Services
{
    ///<summary>
    /// A service to create the HttpClient. 
    ///</summary>
    public class HttpClientService
    {
        public HttpClient Client { get; }

        public HttpClientService(HttpClient client, IConfiguration configuration)
        {
            var apiKey = configuration[ApiSettings.ApiKeyName];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(ExceptionMessage.ApiKeyMissing);
            }

            //The HTTP client is configured in the constructor.
            Client = client;
            Client.BaseAddress = new Uri(UriPath.BaseUri);
            Client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue(HttpClientSettings.JsonType));
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(ApiSettings.BearerScheme, apiKey.Trim());
        }
        public HttpResponseMessage Get(string uri)
        {
            var response = Client.GetAsync(uri).Result;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(ExceptionMessage.ApiKeyRejected);
            }

            return response;
        }
    }
}
