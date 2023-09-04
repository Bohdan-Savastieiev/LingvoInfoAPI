using LanguageStudyAPI.Models;
using LanguageStudyAPI.Models.Lingvo;
using LingvoInfoAPI.DTOs;

namespace LingvoInfoAPI.Mappers
{
    public class LingueeToLingvoInfoMapper : ILingvoInfoMapper<LingueeDto>
    {
        public LingvoInfo MapToLingvoInfo(LingueeDto obj)
        {
            LingvoInfo result = new LingvoInfo()
            {
                Lemma = obj.Text
            };
            foreach (var trans in obj.Translations)
            {
                var lexTrans = new LexemeTranslation() { Text = trans.Text };
                foreach (var examplePair in trans.Examples)
                {
                    lexTrans.Examples.Add(new LexemeExample
                    {
                        NativeExample = examplePair.Src,
                        TranslatedExample = examplePair.Dst
                    });
                }
                result.Translations.Add(lexTrans);
            }

            return result;
        }
    }
}
