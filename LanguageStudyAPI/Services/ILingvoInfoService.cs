using LanguageStudyAPI.Models;

namespace LingvoInfoAPI.Services
{
    public interface ILingvoInfoService
    {
        Task<LingvoInfo> GetTranslationsAsync(string text, string srcLang, string dstLang);
    }
}
