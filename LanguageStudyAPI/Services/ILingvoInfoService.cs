using LanguageStudyAPI.Models;

namespace LingvoInfoAPI.Services
{
    public interface ILingvoInfoService
    {
        Task<LingvoInfo> GetLingvoInfoAsync(string text, string srcLang, string dstLang, bool includeSound);
    }
}
