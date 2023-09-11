using LanguageStudyAPI.Models;
using LanguageStudyAPI.Models.Lingvo;
using LanguageStudyAPI.Services;
using LingvoInfoAPI.Clients;
using LingvoInfoAPI.DTOs;
using LingvoInfoAPI.Mappers;
using LingvoInfoAPI.Models;
using LingvoInfoAPI.Models.RequestParameters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using Newtonsoft.Json.Linq;

namespace LingvoInfoAPI.Services;
public class LingvoInfoService : ILingvoInfoService
{
    private readonly LingvoApiClient _lingvoClient;
    private readonly GoogleTranslationApiClient _googleClient;
    private readonly LingvoTranslationsDtoLingvoInfoMapper _lingvoMapper;
    private readonly LingvoWordFormsDtoMapper _wordFormsMapper;

    private readonly Dictionary<string, int> lingvoLanguages = new ()
    {
        { "zh", 1028 },
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
        GoogleTranslationApiClient googleClient,
        LingvoTranslationsDtoLingvoInfoMapper lingvoMapper,
        LingvoWordFormsDtoMapper wordFormsMapper)
    {
        _lingvoClient = lingvoClient;
        _googleClient = googleClient;
        _lingvoMapper = lingvoMapper;
        _wordFormsMapper = wordFormsMapper;
    }
    public async Task<LingvoInfo> GetTranslationsAsync(string text, string srcLang, string dstLang, bool includeSound)
    {
        string lingvoSrcLang = lingvoLanguages[srcLang].ToString();
        string lingvoDstLang = lingvoLanguages[dstLang].ToString();

        var translations = await _lingvoClient.GetTranslationAsync(text, lingvoSrcLang, lingvoDstLang, false);

        if (translations.IsNullOrEmpty())
        {
            var translation = await _googleClient.TranslateTextAsync(text, dstLang);
            return new LingvoInfo { Lemma = translation };
        }

        var lingvoInfo = _lingvoMapper.MapToLingvoInfo(translations);
        
        var wordFormsDto = await _lingvoClient.GetWordFormsAsync(text, lingvoSrcLang);
        lingvoInfo.WordForms = _wordFormsMapper.MapWordForms(wordFormsDto);

        if (includeSound)
        {
            var sound = await GetSoundAsync(translations);
            if (sound != null)
            {
                lingvoInfo.Sound = sound.EncodedAudio;
            }
        }

        var googleTranslation = await _googleClient.TranslateTextAsync(text, dstLang);
        if(!lingvoInfo.Translations.Any(x => x.Text.Contains(googleTranslation)))
        {
            lingvoInfo.Translations.Insert(0, new LexemeTranslation { Text = googleTranslation });
        }

        return lingvoInfo;
    }
    private async Task<LingvoSoundDto?> GetSoundAsync(List<LingvoTranslationsDto> translations)
    {
        var soundParameters = FindFirstSoundFileAndDictionary(translations);
        if (soundParameters != null)
        {
            var sound = await _lingvoClient.GetSoundAsync(
                soundParameters.DictionaryName,
                soundParameters.FileName);
            return sound;
        }
        return null;
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
