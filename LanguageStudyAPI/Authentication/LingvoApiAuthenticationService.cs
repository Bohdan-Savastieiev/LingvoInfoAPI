using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Security.Authentication;

namespace LanguageStudyAPI.Authentication
{
    public class LingvoApiAuthenticationService : IApiAuthenticationService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly int _tokenExpirationInMinutes;
        private readonly string _authenticateEndpoint;

        public LingvoApiAuthenticationService(
        IHttpClientFactory httpClientFactory,
        IMemoryCache memoryCache,
        IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _memoryCache = memoryCache;

            _client.BaseAddress = new Uri(configuration["LingvoApi:BaseUrl"]
                ?? throw new InvalidOperationException("BaseUrl for Lingvo API is not configured."));

            _apiKey = configuration["LingvoApi:ApiKey"]
                ?? throw new InvalidOperationException("ApiKey for Lingvo API is not configured.");

            _tokenExpirationInMinutes = int.Parse(configuration["LingvoApi:TokenExpirationInMinutes"]
                ?? throw new InvalidOperationException("Token Expiration for Lingvo API is not configured."));

            _authenticateEndpoint = configuration["LingvoApi:AuthenticateEndpoint"]
                ?? throw new InvalidOperationException("Authenticate Endpoint for Lingvo API is not configured.");
        }

        public async Task<string> AuthenticateAsync()
        {
            if (!_memoryCache.TryGetValue("LingvoApiToken", out string token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _apiKey);
                var response = await _client.PostAsync(_authenticateEndpoint, new StringContent(""));
                response.EnsureSuccessStatusCode();

                token = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new AuthenticationException("Lingvo API authentication failed");
                }

                var tokenExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(_tokenExpirationInMinutes));
                _memoryCache.Set("LingvoApiToken", token, tokenExpiration);
            }

            return token;
        }
    }
}