using LanguageStudyAPI.Services;
using Microsoft.Extensions.Configuration;
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

builder.Services.AddScoped<LingueeApiService>();
builder.Services.AddHttpClient<LingueeApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["LingueeApi:BaseUri"]
        ?? throw new ArgumentNullException("Linguee API Base URI wasn't found in configuration settings"));
});

builder.Services.AddScoped<ILingvoApiService, LingvoApiService>();
//builder.Services.AddHttpClient<LingvoApiService>(client =>
//{
//    client.BaseAddress = new Uri(builder.Configuration["LingvoApi:BaseUri"] 
//        ?? throw new ArgumentNullException("Lingvo API Base URI wasn't found in configuration settings"));
//});

builder.Services.AddHttpClient("LingvoApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["LingvoApi:BaseUrl"]
        ?? throw new InvalidOperationException("BaseUrl for Lingvo API is not configured. Please check appsettings.json."));
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