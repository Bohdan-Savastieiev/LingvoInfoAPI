using System.Net;
using LanguageStudyAPI.Models;
using LingvoInfoAPI.DTOs;
using Newtonsoft.Json;

namespace LanguageStudyAPI.Services;

public class LingueeApiService
{
    private readonly HttpClient _httpClient;

    public LingueeApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("LingueeApi");
    }

    public async Task<string> TranslateWordAsync(string query, string src, string dst)
    {
        string requestUrl = $"translations/?query={query}&src={src}&dst={dst}";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.StatusCode == HttpStatusCode.TemporaryRedirect)
        {
            var newUri = response.Headers.Location;
            if (newUri != null)
            {
                response = await _httpClient.GetAsync(newUri);
            }
        }

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            List<LingueeDto> lemmas = JsonConvert.DeserializeObject<List<LingueeDto>>(responseBody);
            return responseBody;
        }
        else
        {
            return string.Empty;
        }
    }
}
