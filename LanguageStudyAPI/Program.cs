using LanguageStudyAPI.Authentication;
using LanguageStudyAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<TranslationService>();

builder.Services.AddScoped<GoogleTranslationAPIService>();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<LingueeApiService>();
builder.Services.AddHttpClient<LingueeApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["LingueeApi:BaseUri"]
        ?? throw new ArgumentNullException("Linguee API Base URI wasn't found in configuration settings"));
});

builder.Services.AddScoped<ILingvoApiService, LingvoApiService>();

builder.Services.AddScoped<LingvoApiAuthenticationHandler>();

builder.Services.AddHttpClient("LingvoApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["LingvoApi:BaseUrl"]
        ?? throw new InvalidOperationException("BaseUrl for Lingvo API is not configured. Please check appsettings.json."));
})
.AddHttpMessageHandler<LingvoApiAuthenticationHandler>();

builder.Services.AddTransient<IApiAuthenticationService>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
    var httpClient = httpClientFactory.CreateClient();

    httpClient.BaseAddress = new Uri(builder.Configuration["LingvoApi:BaseUrl"])
        ?? throw new InvalidOperationException("ApiKey for Lingvo API is not configured. Please check appsettings.json.");

    string apiKey = builder.Configuration["LingvoApi:ApiKey"]
        ?? throw new InvalidOperationException("ApiKey for Lingvo API is not configured. Please check appsettings.json.");

    string tokenExpirationInMinutesString = builder.Configuration["LingvoApi:TokenExpirationInMinutes"]
        ?? throw new InvalidOperationException("ApiKey for Lingvo API is not configured. Please check appsettings.json.");
    DateTimeOffset tokenExpirationInMinutes = new DateTimeOffSet(tokenExpirationInMinutesString);

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