using LanguageStudyAPI.Models.Lingvo;
using LanguageStudyAPI.Services;
using LingvoInfoAPI.DTOs;
using Newtonsoft.Json;
using System.Net;

namespace LingvoInfoAPI.Clients
{
    public class LingvoApiClient
    {
        private readonly HttpClient _client;
        private readonly string _translationEndpoint;
        private readonly string _wordFormsEndpoint;
        private readonly string _soundEndpoint;

        // Centralized configuration keys
        private const string TranslationEndpointKey = "LingvoApi:TranslationEndpoint";
        private const string WordFormsEndpointKey = "LingvoApi:WordFormsEndpoint";
        private const string SoundEndpointKey = "LingvoApi:SoundEndpoint";

        public LingvoApiClient(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _client = clientFactory.CreateClient("LingvoApi");
            ValidateConfiguration(configuration);

            _translationEndpoint = configuration[TranslationEndpointKey];
            _wordFormsEndpoint = configuration[WordFormsEndpointKey];
            _soundEndpoint = configuration[SoundEndpointKey];
        }

        private async Task<T> MakeApiRequestAsync<T>(string requestUrl)
        {
            HttpResponseMessage response = await _client.GetAsync(requestUrl);
            return HandleApiResponse<T>(response);
        }

        private T HandleApiResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(content);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default(T);
            }
            else
            {
                throw new HttpRequestException($"Error calling Lingvo API: {response.ReasonPhrase}");
            }
        }

        public async Task<List<LingvoTranslationsDto>> GetTranslationAsync(string text, string srcLang, string dstLang, bool isCaseSensitive)
        {
            string requestUrl = $"{_translationEndpoint}?text={WebUtility.UrlEncode(text)}&srcLang={WebUtility.UrlEncode(srcLang)}&dstLang={WebUtility.UrlEncode(dstLang)}&isCaseSensitive={isCaseSensitive}";
            var result = await MakeApiRequestAsync<List<LingvoTranslationsDto>>(requestUrl);
            return result ?? new List<LingvoTranslationsDto>();
        }

        public async Task<List<LingvoWordFormsDto>> GetWordFormsAsync(string text, string lang)
        {
            string requestUrl = $"{_wordFormsEndpoint}?text={WebUtility.UrlEncode(text)}&lang={WebUtility.UrlEncode(lang)}";
            var result = await MakeApiRequestAsync<List<LingvoWordFormsDto>>(requestUrl);
            return result ?? new List<LingvoWordFormsDto>();
        }

        public async Task<LingvoSoundDto?> GetSoundAsync(string dictionaryName, string fileName)
        {
            string requestUrl = $"{_soundEndpoint}?dictionaryName={WebUtility.UrlEncode(dictionaryName)}&fileName={WebUtility.UrlEncode(fileName)}";
            var sound = await MakeApiRequestAsync<LingvoSoundDto>(requestUrl);
            if (sound != null)
            {
                // Explanation for the replacement (assuming it's a known issue)
                sound.EncodedAudio = sound.EncodedAudio.Replace("\"", "");
            }
            return sound;
        }

        private void ValidateConfiguration(IConfiguration configuration)
        {
            ValidateEndpointConfiguration(configuration, TranslationEndpointKey);
            ValidateEndpointConfiguration(configuration, WordFormsEndpointKey);
            ValidateEndpointConfiguration(configuration, SoundEndpointKey);
        }

        private void ValidateEndpointConfiguration(IConfiguration configuration, string key)
        {
            if (string.IsNullOrEmpty(configuration[key]))
            {
                throw new InvalidOperationException($"{key} for Lingvo API is not configured.");
            }
            // Additional validation for valid URLs can be added here if necessary
        }
    }
}
