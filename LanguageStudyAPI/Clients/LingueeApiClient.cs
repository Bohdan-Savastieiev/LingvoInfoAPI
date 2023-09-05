using LingvoInfoAPI.DTOs;
using Newtonsoft.Json;
using System.Net;

namespace LingvoInfoAPI.Clients;
public class LingueeApiClient
{
    private readonly HttpClient _httpClient;

    public LingueeApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("LingueeApi");
    }

    public async Task<List<LingueeDto>> TranslateWordAsync(string query, string src, string dst)
    {
        string requestUrl = $"translations/?query={query}&src={src}&dst={dst}";
        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        //if (response.StatusCode == HttpStatusCode.TemporaryRedirect)
        //{
        //    var newUri = response.Headers.Location;
        //    if (newUri != null)
        //    {
        //        response = await _httpClient.GetAsync(newUri);
        //    }
        //}

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            List<LingueeDto> result = JsonConvert.DeserializeObject<List<LingueeDto>>(responseBody);
            return result;
        }
        else if (response.StatusCode == HttpStatusCode.InternalServerError
            && response.Content.ToString() == "Translation not found")
        {
            return new List<LingueeDto>();
        }
        else
        {
            throw new HttpRequestException(response.Content.ToString());
        }
    }
}
