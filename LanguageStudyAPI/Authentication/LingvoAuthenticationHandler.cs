using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace LanguageStudyAPI.Authentication
{
    public class LingvoAuthenticationHandler : DelegatingHandler
    {
        private HttpClient _client;
        private readonly string _apiKey;
        private string _bearerToken;
        private DateTime _tokenExpiry;

        public LingvoAuthenticationHandler(HttpClient client, string apiKey)
        {
            _client = client;
            _apiKey = apiKey;
        }

        private async Task<bool> AuthenticateAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _apiKey);
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1.1/authenticate");
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                _bearerToken = await response.Content.ReadAsStringAsync();
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = tokenHandler.ReadJwtToken(_bearerToken);
                _tokenExpiry = token.ValidTo;

                return true;
            }
            return false;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_tokenExpiry <= DateTime.UtcNow.AddSeconds(15))
            {
                bool isAuthenticated = await AuthenticateAsync();
                if (!isAuthenticated)
                {
                    // Обработка ошибки аутентификации
                }
            }
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
