using System.Net.Http.Headers;

namespace LanguageStudyAPI.Authentication
{
    public class LingvoApiAuthenticationHandler : DelegatingHandler
    {
        private IApiAuthenticationService _authService;
        private string _bearerToken = null!;

        public LingvoApiAuthenticationHandler(IApiAuthenticationService authService)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _bearerToken = await _authService.AuthenticateAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
