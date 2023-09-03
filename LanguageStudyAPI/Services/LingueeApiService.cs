using System.Net;
using LanguageStudyAPI.Models.Linguee;
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

        // Check for redirect status code
        if (response.StatusCode == HttpStatusCode.TemporaryRedirect)
        {
            // Get the new URI from the Location header
            var newUri = response.Headers.Location;
            if (newUri != null)
            {
                // Make a new request to the redirected URI
                response = await _httpClient.GetAsync(newUri);
            }
        }

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            List<LingueeModel> lemmas = JsonConvert.DeserializeObject<List<LingueeModel>>(responseBody);
            return responseBody;
        }
        else
        {
            return string.Empty;
        }
    }
}
