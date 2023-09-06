using LingvoInfoAPI.DTOs;

namespace LanguageStudyAPI.Models;
public class LingvoInfo
{
    public string Lemma { get; set; }
    public string? Transcription { get; set; }
    public string? Sound { get; set; }
    public List<LexemeTranslation> Translations { get; set; }
    public List<WordForm> WordForms { get; set; }

    public LingvoInfo()
    {
        Translations = new List<LexemeTranslation>();
        WordForms = new List<WordForm>();
    }
}
public class LexemeTranslation
{
    public string Text { get; set; }
    public List<LexemeExample> Examples { get; set; }
    public LexemeTranslation()
    {
        Examples = new List<LexemeExample>();
    }
}
public class LexemeExample
{
    public string NativeExample { get; set; }
    public string TranslatedExample { get; set; }
}
public class WordForm
{
    public string Text { get; set; }
}
