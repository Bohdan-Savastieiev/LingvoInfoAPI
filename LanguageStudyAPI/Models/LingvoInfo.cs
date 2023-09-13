using LingvoInfoAPI.DTOs;

namespace LanguageStudyAPI.Models;
public class LingvoInfo
{
    public string Lemma { get; set; }
    public string? Transcription { get; set; }
    public List<LexemeTranslation> Translations { get; set; }
    public List<WordForm> WordForms { get; set; }
    public string? Sound { get; set; }

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
    public List<Synonym> Synonyms { get; set; }
    public List<Antonym> Antonyms { get; set; }
    public List<DerivedLexeme> DerivedLexemes { get; set; }

    public LexemeTranslation()
    {
        Examples = new List<LexemeExample>();
        Synonyms = new List<Synonym>();
        Antonyms = new List<Antonym>();
        DerivedLexemes = new List<DerivedLexeme>();
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
public class Synonym
{
    public string Text { get; set; }
}
public class Antonym
{
    public string Text { get; set; }
}
public class DerivedLexeme
{
    public string Text { get; set; }
    public string Scope { get; set; }
}
