namespace LingvoInfoAPI.Services
{
    public interface ILanguageCheckService
    {
        bool AreSameLanguages(string text1, string text2);
    }
}
