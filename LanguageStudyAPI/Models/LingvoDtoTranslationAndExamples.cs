using LanguageStudyAPI.Models.Lingvo;

namespace LingvoInfoAPI.Models
{
    public class LingvoDtoTranslationAndExamples
    {
        public ParagraphNode Translation { get; set; }
        public ExamplesNode? Examples { get; set; }
    }
}
