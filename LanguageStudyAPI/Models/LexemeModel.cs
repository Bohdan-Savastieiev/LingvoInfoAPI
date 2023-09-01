namespace LanguageStudyAPI.Models
{
    public class LexemeModel
    {
        public string Text { get; set; }
        public string Transcription { get; set; }
        public string Sound { get; set; }
        public List<LexemeExample> Examples { get; set; }
        public List<LexemeTranslation> Translations { get; set; }

        public LexemeModel()
        {
            Examples = new List<LexemeExample>();
            Translations = new List<LexemeTranslation>();
        }

    }
    public class LexemeExample
    {
        public string NativeExample { get; set; }
        public string TranslatedExample { get; set; }
    }
    public class LexemeTranslation
    {
        public string Text { get; set; }
        public string Transcription { get; set; }

    }
}
