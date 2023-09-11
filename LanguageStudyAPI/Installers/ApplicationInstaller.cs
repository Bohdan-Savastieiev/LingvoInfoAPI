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
                    ?? throw new InvalidOperationException("Linguee API Base URI wasn't found in configuration settings"));
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
    }
}
