using LanguageStudyAPI.Authentication;
using LanguageStudyAPI.Services;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<GoogleTranslationAPIService>();

builder.Services.AddScoped<LingueeApiService>();
builder.Services.AddHttpClient<LingueeApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["LingueeApi:BaseUrl"]
        ?? throw new ArgumentNullException("Linguee API Base URI wasn't found in configuration settings"));
});

builder.Services.AddScoped<ILingvoApiService, LingvoApiService>();

builder.Services.AddScoped<LingvoApiAuthenticationHandler>();

builder.Services.AddHttpClient("LingvoApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["LingvoApi:BaseUrl"]
        ?? throw new InvalidOperationException("BaseUrl for Lingvo API is not configured."));
})
.AddHttpMessageHandler<LingvoApiAuthenticationHandler>();

builder.Services.AddTransient<IApiAuthenticationService>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
    var httpClient = httpClientFactory.CreateClient();

    string baseUrl = builder.Configuration["LingvoApi:BaseUrl"]
        ?? throw new InvalidOperationException("BaseUrl for Lingvo API is not configured.");
    httpClient.BaseAddress = new Uri(baseUrl);

    string apiKey = builder.Configuration["LingvoApi:ApiKey"]
        ?? throw new InvalidOperationException("ApiKey for Lingvo API is not configured.");

    string tokenExpirationInMinutesString = builder.Configuration["LingvoApi:TokenExpirationInMinutes"]
        ?? throw new InvalidOperationException("Token Expiration for Lingvo API is not configured.");
    int tokenExpirationInMinutes = int.Parse(tokenExpirationInMinutesString);

    return new LingvoApiAuthenticationService(httpClient, memoryCache, apiKey, tokenExpirationInMinutes);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }