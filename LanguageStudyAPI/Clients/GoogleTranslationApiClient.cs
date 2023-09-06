using Google.Cloud.Translation.V2;

namespace LingvoInfoAPI.Clients
{
    public class GoogleTranslationApiClient
    {
        private readonly TranslationClient _translationClient;

        public GoogleTranslationApiClient(IConfiguration configuration)
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
