using LanguageStudyAPI.Authentication;
using LanguageStudyAPI.Converters;
using LanguageStudyAPI.Mappers;
using LanguageStudyAPI.Services;
using LingvoInfoAPI.Clients;
using LingvoInfoAPI.Factories;
using LingvoInfoAPI.Installers;
using LingvoInfoAPI.Mappers;
using LingvoInfoAPI.Services;
using Mapster;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Controllers and API documentation
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cache
builder.Services.AddMemoryCache();

// Factories
builder.Services.AddSingleton<INodeFactory, LingvoNodeFactory>();

//Converters
builder.Services.AddSingleton<LingvoJsonConverter>();

// Mappers
MapsterConfig.Configure();
builder.Services.AddScoped<LingvoTranslationsDtoLingvoInfoMapper>();
builder.Services.AddScoped<LingvoWordFormsDtoMapper>();

// API Clients
builder.Services.AddScoped<GoogleTranslationApiClient>();
builder.Services.AddScoped<LingvoApiClient>();

// Services
builder.Services.AddScoped<ILingvoInfoService, LingvoInfoService>();
builder.Services.AddScoped<GoogleTranslationApiService>();

// Http Clients Configuration
ApplicationInstaller.ConfigureLingueeHttpClient(builder);
ApplicationInstaller.ConfigureLingvoHttpClient(builder);

// Authentication
builder.Services.AddScoped<LingvoApiAuthenticationHandler>();
builder.Services.AddTransient<IApiAuthenticationService, LingvoApiAuthenticationService>();

var app = builder.Build();

// Development-specific configurations
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