using LanguageStudyAPI.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace LanguageStudyAPI.Tests
{
    public class LingvoTests
    {
    //    [Fact]
    //    public async Task AuthenticateAsync_ShouldReturnTrue_WhenAuthenticationIsSuccessful()
    //    {
    //        // Arrange
    //        var mockFactory = new Mock<IHttpClientFactory>();
    //        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    //        mockHttpMessageHandler
    //            .Protected()
    //            .Setup<Task<HttpResponseMessage>>(
    //                "SendAsync",
    //                ItExpr.IsAny<HttpRequestMessage>(),
    //                ItExpr.IsAny<CancellationToken>()
    //            )
    //            .ReturnsAsync(new HttpResponseMessage
    //            {
    //                StatusCode = HttpStatusCode.OK,
    //                Content = new StringContent("\"someToken\""),
    //            });

    //        var client = new HttpClient(mockHttpMessageHandler.Object)
    //        {
    //            BaseAddress = new Uri("https://developers.lingvolive.com/")
    //        };
    //        mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

    //        var config = new ConfigurationBuilder()
    //            .AddInMemoryCollection(new List<KeyValuePair<string, string>> {
    //        new KeyValuePair<string, string>("LingvoApi:ApiKey", "someApiKey")
    //            })
    //            .Build();

    //        var service = new LingvoApiService(mockFactory.Object, config);

    //        // Act
    //        var result = await service.AuthenticateAsync();

    //        // Assert
    //        Assert.True(result);
    //    }

        [Theory]
        [InlineData("envelope.json")]
        [InlineData("horizon.json")]
        [InlineData("kill.json")]
        [InlineData("love.json")]
        [InlineData("me.json")]
        [InlineData("son.json")]
        [InlineData("RuTest1.json")]
        [InlineData("RuTest2.json")]
        [InlineData("RuTest3.json")]
        [InlineData("RuTest4.json")]
        public async Task FindAllExampleNodes_WorksCorrectly(string jsonFileName)
        {
            // Arrange
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), $"../../../TestFiles/FindExamplesFiles/{jsonFileName}");
            string json = await File.ReadAllTextAsync(jsonPath);
            JToken token = JToken.Parse(json);

            // Act
            var resultList = LingvoApiService.FindAllExampleNodes(token);
            var resultJArray = new JArray(resultList);

            // Assert
            foreach (var jToken in resultJArray)
            {
                var jObject = jToken as JObject;
                Assert.NotNull(jObject);
            }

            // Write result to file
            var outputJson = resultJArray.ToString();
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestFiles/FindExamplesResults", jsonFileName);
            await File.WriteAllTextAsync(outputPath, outputJson);
        }

        [Theory]
        [InlineData("envelope.json")]
        [InlineData("horizon.json")]
        [InlineData("kill.json")]
        [InlineData("love.json")]
        [InlineData("me.json")]
        [InlineData("son.json")]
        [InlineData("RuTest1.json")]
        [InlineData("RuTest2.json")]
        [InlineData("RuTest3.json")]
        [InlineData("RuTest4.json")]
        public async Task ConvertToLexemeExamples_WorksCorrectly(string jsonFileName)
        {
            // Arrange
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestFiles/FindExamplesResults/", jsonFileName);
            var json = await File.ReadAllTextAsync(jsonPath);
            var token = JToken.Parse(json);
            var examples = new List<JToken>(token);

            // Act
            var result = LingvoApiService.ConvertToLexemeExamples(examples);

            // Assert
            foreach (var example in result)
            {
                Assert.NotNull(example);
                Assert.NotNull(example.NativeExample);
                Assert.NotNull(example.TranslatedExample);
            }

            // Write result to file
            var outputJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestFiles/ConvertedResults/", jsonFileName);
            await File.WriteAllTextAsync(outputPath, outputJson);
        }

    }
}