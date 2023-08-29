namespace LanguageStudyAPI.Services
{
    public static class TranslationService
    {
        public static int CountWordsInString(string lexeme)
        {
            var dividers = new[] { ' ', '.', ',', '!', '?' };
            var words = lexeme.Split(dividers, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
    }
}
