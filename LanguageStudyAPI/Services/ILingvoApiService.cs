using LanguageStudyAPI.Models;

namespace LanguageStudyAPI.Services
{
    public interface ILingvoApiService
    {
        //Task<bool> AuthenticateAsync();
        Task<string> GetTranslationAsync(string text, string srcLang, string dstLang, bool isCaseSensitive);
        Task<List<LexemeExample>> GetExamplesFromTranslationAsync(string text, string srcLang, string dstLang, bool isCaseSensitive);

    }
}
