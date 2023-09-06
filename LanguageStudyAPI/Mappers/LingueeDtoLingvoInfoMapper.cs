using LanguageStudyAPI.Models;
using LingvoInfoAPI.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace LingvoInfoAPI.Mappers
{
    public class LingueeDtoLingvoInfoMapper : ILingvoInfoMapper<LingueeDto>
    {
        public LingvoInfo MapToLingvoInfo(List<LingueeDto> obj)
        {
            LingvoInfo result = new LingvoInfo();
            foreach (var objItem in obj)
            {
                MapSingleDtoToLingvoInfo(result, objItem);
            }

            return result;
        }
        public LingvoInfo MapToLingvoInfo(LingueeDto linguee)
        {
            var result = new LingvoInfo()
            { 
                Lemma = linguee.Text 
            };
            MapTranslationsAndExamples(result, linguee);

            return result;
        }

        private void MapSingleDtoToLingvoInfo(LingvoInfo result, LingueeDto linguee)
        {
            if (result.Lemma.IsNullOrEmpty() || linguee.Text != result.Lemma)
            {
                result.Lemma = linguee.Text;
                MapTranslationsAndExamples(result, linguee);
            }
        }

        private void MapTranslationsAndExamples(LingvoInfo result, LingueeDto linguee)
        {
            foreach (var trans in linguee.Translations)
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
        }
    }
}
