using LanguageStudyAPI.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using LanguageStudyAPI.Controllers;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageStudyAPI.Tests;
public class LingvoControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ILingvoApiService> _mockService;

    public LingvoControllerIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _mockService = new Mock<ILingvoApiService>();
    }

    [Fact]
    public async Task GetTranslation_ReturnsOk()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => _mockService.Object);
            });
        }).CreateClient();

        _mockService.Setup(x => x.GetTranslationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                    .ReturnsAsync("TestTranslation");

        // Act
        var response = await client.GetAsync("api/Lingvo/Translation?text=test&srcLang=en&dstLang=ru&isCaseSensitive=false");

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal("TestTranslation", stringResponse);
    }

}
