using LanguageStudyAPI.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace LingvoInfoAPI.Installers
{
    public static class ApplicationInstaller
    {
        public static void ConfigureLingueeHttpClient(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient("LingueeApi", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["LingueeApi:BaseUrl"]
                    ?? throw new ArgumentNullException("Linguee API Base URI wasn't found in configuration settings"));
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 5
            });
        }
        public static void ConfigureLingvoHttpClient(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient("LingvoApi", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["LingvoApi:BaseUrl"]
                    ?? throw new InvalidOperationException("BaseUrl for Lingvo API is not configured."));
            })
            .AddHttpMessageHandler<LingvoApiAuthenticationHandler>();
        }
        public static IApiAuthenticationService CreateLingvoApiAuthenticationService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            var httpClient = httpClientFactory.CreateClient();

            string baseUrl = configuration["LingvoApi:BaseUrl"]
                ?? throw new InvalidOperationException("BaseUrl for Lingvo API is not configured.");
            httpClient.BaseAddress = new Uri(baseUrl);

            string apiKey = configuration["LingvoApi:ApiKey"]
                ?? throw new InvalidOperationException("ApiKey for Lingvo API is not configured.");

            string tokenExpirationInMinutesString = configuration["LingvoApi:TokenExpirationInMinutes"]
                ?? throw new InvalidOperationException("Token Expiration for Lingvo API is not configured.");
            int tokenExpirationInMinutes = int.Parse(tokenExpirationInMinutesString);

            return new LingvoApiAuthenticationService(httpClient, memoryCache, apiKey, tokenExpirationInMinutes);
        }
    }
}
