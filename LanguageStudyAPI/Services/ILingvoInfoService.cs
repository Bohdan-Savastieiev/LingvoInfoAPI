namespace LingvoInfoAPI.Services
{
    public interface ILingvoInfoService
    {
        Task<string> GetTranslationsAsync(string text, string srcLang, string dstLang);
    }
}
