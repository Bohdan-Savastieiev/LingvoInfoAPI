using LanguageStudyAPI.Services;

namespace LingvoInfoAPI.Services;
public class LingvoInfoService : ILingvoInfoService
{
    private readonly ILingvoApiService _lingvoService;
    private readonly LingueeApiService _lingueeService;
    private readonly GoogleTranslationAPIService _googleService;
    public LingvoInfoService(
        ILingvoApiService lingvoService,
        LingueeApiService lingueeService,
        GoogleTranslationAPIService googleService)
    {
        _lingvoService = lingvoService;
        _lingueeService = lingueeService;
        _googleService = googleService;
    }
    public Task<string> GetTranslationsAsync(string text, string srcLang, string dstLang)
    {
        throw new NotImplementedException();
    }
}
