using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Configuration;

namespace LanguageStudyAPI.Services
{
    public class GoogleTranslationApiService
    {
        private readonly TranslationClient _translationClient;

        public GoogleTranslationApiService(IConfiguration configuration)
        {
            var apiKey = configuration["GoogleCloudSettings:ApiKey"];
            _translationClient = TranslationClient.CreateFromApiKey(apiKey);
        }

        public async Task<string> TranslateTextAsync(string text, string targetLanguage)
        {
            TranslationResult result = await _translationClient.TranslateTextAsync(text, targetLanguage);
            return result.TranslatedText;
        }

        public async Task<IList<string>> TranslateMultipleAsync(string[] texts, string targetLanguage)
        {
            var translations = new List<string>();

            foreach (var text in texts)
            {
                TranslationResult result = await _translationClient.TranslateTextAsync(text, targetLanguage);
                translations.Add(result.TranslatedText);
            }

            return translations;
        }
    }
}
