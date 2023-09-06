using LanguageStudyAPI.Models.Lingvo;
using LanguageStudyAPI.Services;
using LingvoInfoAPI.DTOs;
using Newtonsoft.Json;

namespace LingvoInfoAPI.Clients
{
    public class LingvoApiClient
    {
        private readonly HttpClient _client;
        private readonly Dictionary<string, int> lingvoLanguages = new Dictionary<string, int>()
    {
        { "ch", 1028 },
        { "de", 1031 },
        { "el", 1032 },
        { "en", 1033 },
        { "es", 1034 },
        { "fr", 1036 },
        { "it", 1040 },
        { "ru", 1049 },
        { "uk", 1058 }
    };
        public LingvoApiClient(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("LingvoApi");
        }

        private async Task<T> MakeApiRequestAsync<T>(string requestUrl)
        {
            HttpResponseMessage response = await _client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                throw new HttpRequestException($"Error calling Lingvo API: {response.ReasonPhrase}");
            }
        }

        public async Task<List<LingvoTranslationsDto>> GetTranslationAsync(string text, string srcLang, string dstLang, bool isCaseSensitive)
        {
            string requestUrl = $"api/v1/Translation?text={text}&srcLang={srcLang}&dstLang={dstLang}&isCaseSensitive={isCaseSensitive}";
            return await MakeApiRequestAsync<List<LingvoTranslationsDto>>(requestUrl);
        }

        public async Task<List<LingvoWordFormsDto>> GetWordFormsAsync(string text, string lang)
        {
            string requestUrl = $"api/v1/WordForms?text={text}&lang={lang}";
            return await MakeApiRequestAsync<List<LingvoWordFormsDto>>(requestUrl);
        }

        public async Task<LingvoSoundDto> GetSoundAsync(string dictionaryName, string fileName)
        {
            string requestUrl = $"api/v1/Sound?dictionaryName={dictionaryName}&fileName={fileName}";
            HttpResponseMessage response = await _client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var result = new LingvoSoundDto(){ EncodedAudio = content.Replace("\"", "") };
                return result;
            }
            else
            {
                throw new HttpRequestException($"Error calling Lingvo API: {response.ReasonPhrase}");
            }
        }

    }

}