using LanguageStudyAPI.Models;
using LanguageStudyAPI.Models.Lingvo;
using LanguageStudyAPI.Services;
using LingvoInfoAPI.Clients;
using LingvoInfoAPI.DTOs;
using LingvoInfoAPI.Mappers;
using LingvoInfoAPI.Models.RequestParameters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using Newtonsoft.Json.Linq;

namespace LingvoInfoAPI.Services;
public class LingvoInfoService : ILingvoInfoService
{
    private readonly LingvoApiClient _lingvoClient;
    private readonly LingueeApiClient _lingueeClient;
    private readonly GoogleTranslationApiClient _googleClient;
    private readonly LingvoTranslationsDtoLingvoInfoMapper _lingvoMapper;
    private readonly LingueeDtoLingvoInfoMapper _lingueeMapper;
    private readonly LingvoWordFormsDtoMapper _wordFormsMapper;

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
        GoogleTranslationApiClient googleClient,
        LingvoTranslationsDtoLingvoInfoMapper lingvoMapper,
        LingueeDtoLingvoInfoMapper lingueeMapper,
        LingvoWordFormsDtoMapper wordFormsMapper)
    {
        _lingvoClient = lingvoClient;
        _lingueeClient = lingueeClient;
        _googleClient = googleClient;
        _lingvoMapper = lingvoMapper;
        _lingueeMapper = lingueeMapper;
        _wordFormsMapper = wordFormsMapper;
    }
    public async Task<LingvoInfo> GetTranslationsAsync(string text, string srcLang, string dstLang)
    {
        string lingvoSrcLang = lingvoLanguages[srcLang].ToString();
        string lingvoDstLang = lingvoLanguages[dstLang].ToString();

        var translations = await _lingvoClient.GetTranslationAsync(text, lingvoSrcLang, lingvoDstLang, false);

        var lingvoInfo = _lingvoMapper.MapToLingvoInfo(translations);

        var soundParameters = FindFirstSoundFileAndDictionary(translations);
        if (soundParameters != null)
        {
            var sound = await _lingvoClient.GetSoundAsync(
                soundParameters.DictionaryName, 
                soundParameters.FileName);
            //lingvoInfo.Sound = sound.EncodedAudio;
        }

        var wordFormsDto = await _lingvoClient.GetWordFormsAsync(text, lingvoSrcLang);
        lingvoInfo.WordForms = _wordFormsMapper.MapWordForms(wordFormsDto);

        return lingvoInfo;
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
