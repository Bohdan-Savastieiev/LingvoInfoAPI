namespace LanguageStudyAPI.Models
{
    public class LexemeModel
    {
        public string Text;
        public List<LexemeExample> Examples;
        public LexemeTranslation Translation;

    }
    public class LexemeExample
    {
        public string NativeExample;
        public string TranslatedExample;
    }
    public class LexemeTranslation
    {
        public string Text;
        public string Transcription;

    }
}
