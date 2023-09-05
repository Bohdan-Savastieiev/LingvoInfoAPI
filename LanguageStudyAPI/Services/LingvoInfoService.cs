using LanguageStudyAPI.Models.Lingvo;
using LanguageStudyAPI.Services;
using LingvoInfoAPI.Clients;
using LingvoInfoAPI.DTOs;
using LingvoInfoAPI.Models.RequestParameters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace LingvoInfoAPI.Services;
public class LingvoInfoService : ILingvoInfoService
{
    private readonly LingvoApiClient _lingvoClient;
    private readonly LingueeApiClient _lingueeClient;
    private readonly GoogleTranslationAPIClient _googleClient;
    private readonly Dictionary<string, int> lingvoLanguages = new ()
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
    public LingvoInfoService(
        LingvoApiClient lingvoClient,
        LingueeApiClient lingueeClient,
        GoogleTranslationAPIClient googleClient)
    {
        _lingvoClient = lingvoClient;
        _lingueeClient = lingueeClient;
        _googleClient = googleClient;
    }
    public async Task<string> GetTranslationsAsync(string text, string srcLang, string dstLang)
    {
        string lingvoSrcLang = lingvoLanguages[srcLang].ToString();
        string lingvoDstLang = lingvoLanguages[dstLang].ToString();

        var translations = await _lingvoClient.GetTranslationAsync(text, lingvoSrcLang, lingvoDstLang, false);

        LingvoSoundDto? sound;
        var soundParameters = FindFirstSoundFileAndDictionary(translations);
        if (soundParameters != null)
        {
            sound = await _lingvoClient.GetSoundAsync(
                soundParameters.DictionaryName, 
                soundParameters.FileName);
        }

        var wordForms = await _lingvoClient.GetWordFormsAsync(text, lingvoSrcLang);

        return "";
    }
    private LingvoSoundParameters? FindFirstSoundFileAndDictionary(List<LingvoTranslationsDto> translations)
    {   
        foreach (var translation in translations)
        {
            var paragraphNode = translation.Body.FirstOrDefault(x => x is ParagraphNode) as ParagraphNode;
            if (paragraphNode == null)
            {
                continue;
            }

            var soundNode = paragraphNode.Markup.FirstOrDefault(x => x is SoundNode) as SoundNode;
            if (soundNode != null && !soundNode.FileName.IsNullOrEmpty())
            {
                return new LingvoSoundParameters { DictionaryName = translation.Dictionary, FileName = soundNode.FileName };
            }
        }

        return null;
    }

}
