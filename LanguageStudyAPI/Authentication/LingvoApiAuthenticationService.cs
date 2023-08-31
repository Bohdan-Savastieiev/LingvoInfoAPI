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
        private readonly int _tokenLongivity;
        public LingvoApiAuthenticationService(HttpClient client,
            IMemoryCache memoryCache,
            string apiKey,
            int tokenLongivity)
        {
            _client = client;
            _memoryCache = memoryCache;
            _apiKey = apiKey;
            _tokenLongivity = tokenLongivity;
        }

        public async Task<string> AuthenticateAsync()
        {
            if (!_memoryCache.TryGetValue("LingvoApiToken", out string token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _apiKey);
                var response = await _client.PostAsync("api/v1.1/authenticate", new StringContent(""));
                response.EnsureSuccessStatusCode();

                token = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new AuthenticationException("Lingvo API authentication failed");
                }

                var tokenExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(_tokenLongivity));
                _memoryCache.Set("LingvoApiToken", token, tokenExpiration);
            }

            return token;
        }
    }
}