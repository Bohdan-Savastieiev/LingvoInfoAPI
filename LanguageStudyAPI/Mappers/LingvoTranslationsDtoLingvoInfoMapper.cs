using Google.Cloud.Translation.V2;
using LanguageStudyAPI.Models;
using LanguageStudyAPI.Models.Lingvo;
using LingvoInfoAPI.DTOs;
using LingvoInfoAPI.Models;
using LingvoInfoAPI.Models.RequestParameters;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

namespace LingvoInfoAPI.Mappers
{
    public class LingvoTranslationsDtoLingvoInfoMapper : ILingvoInfoMapper<LingvoTranslationsDto>
    {
        public LingvoInfo MapToLingvoInfo(LingvoTranslationsDto translations)
        {
            throw new NotImplementedException();
        }
        public LingvoInfo MapToLingvoInfo(List<LingvoTranslationsDto> translations)
        {
            if (translations.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(translations));
            }

            var lingvoInfo = new LingvoInfo();
            lingvoInfo.Lemma = translations.First().Title;
            MapTranscription(lingvoInfo, translations);
            MapTranslations(lingvoInfo, translations);

            return lingvoInfo;

        }

        private void MapTranscription(LingvoInfo lingvoInfo, List<LingvoTranslationsDto> translations)
        {
            foreach (var dtoObj in translations)
            {
                var transcriptionNode = GetTranscriptionNode(dtoObj);
                if (transcriptionNode != null && !transcriptionNode.Text.IsNullOrEmpty())
                {
                    lingvoInfo.Transcription = transcriptionNode.Text;
                    return;
                }
            }
        }

        private TranscriptionNode? GetTranscriptionNode(LingvoTranslationsDto translation)
        {
            var paragraphNode = translation.Body.FirstOrDefault(x => x is ParagraphNode) as ParagraphNode;
            if (paragraphNode == null)
            {
                return null;
            }

            return paragraphNode.Markup.FirstOrDefault(x => x is TranscriptionNode) as TranscriptionNode;
        }
        private List<LingvoDtoTranslationAndExamples>? GetTranslationAndExamples(LingvoTranslationsDto dtoObj)
        {
            var result = new List<LingvoDtoTranslationAndExamples>();
            var paragraphNode = dtoObj.Body.OfType<ListNode>().FirstOrDefault();
            if (paragraphNode == null)
            {
                // check if there is one single translation and not a list of translations
                var translationParagraphNode = dtoObj.Body
                    .OfType<ParagraphNode>()
                    .FirstOrDefault(paragraph => paragraph.Markup.FirstOrDefault() is TextNode);

                if (translationParagraphNode != null)
                {
                    var examplesNode = dtoObj.Body
                                        .OfType<ExamplesNode>()
                                        .FirstOrDefault();

                    result.Add(new LingvoDtoTranslationAndExamples { Translation = translationParagraphNode, Examples = examplesNode });
                }
            }
            else
            {
                foreach (var listItemNode in paragraphNode.Items)
                {
                    if (listItemNode == null)
                    {
                        continue;
                    }

                    var translationParagraphNode = listItemNode.Markup
                        .OfType<ParagraphNode>()
                        .FirstOrDefault(paragraph => paragraph.Markup.FirstOrDefault() is TextNode);
                    if (translationParagraphNode == null)
                    {
                        // || translationParagraphNode.Markup.Count != 1
                        continue;
                    }

                    var examplesNode = listItemNode.Markup
                        .OfType<ExamplesNode>()
                        .FirstOrDefault();

                    result.Add(new LingvoDtoTranslationAndExamples { Translation = translationParagraphNode, Examples = examplesNode });
                }
            }

            return result;
        }

        private void MapTranslations(LingvoInfo lingvoInfo, List<LingvoTranslationsDto> dtoObjs)
        {
            foreach (var dtoObj in dtoObjs)
            {
                var paragraphNode = dtoObj.Body.FirstOrDefault(x => x is ListNode) as ListNode;
                if (paragraphNode == null)
                {
                    // check if there is one single translation and not a list of translations
                    var paragraphTranslationNode = dtoObj.Body.Where(x => x is ParagraphNode paragraph
                        && paragraph.Markup.FirstOrDefault() is TextNode).FirstOrDefault() as ParagraphNode;
                    if (paragraphTranslationNode != null && paragraphTranslationNode.Markup != null)
                    {
                        var translationNode = paragraphTranslationNode.Markup.FirstOrDefault();
                        if (translationNode != null && !translationNode.Text.IsNullOrEmpty())
                        {
                            var translation = new LexemeTranslation()
                            {
                                Text = translationNode.Text
                            };
                            lingvoInfo.Translations.Add(translation);
                        }


                    }

                    continue;
                }

                foreach (var listItemNode in paragraphNode.Items)
                {
                    if (listItemNode == null)
                    {
                        continue;
                    }

                    var translationParagraphNode = listItemNode.Markup.FirstOrDefault(x => x is ParagraphNode) as ParagraphNode;
                    if (translationParagraphNode == null || translationParagraphNode.Markup.Count != 1)
                    {
                        continue;
                    }

                    var translationNode = translationParagraphNode.Markup.FirstOrDefault(x => x is TextNode) as TextNode;
                    if (translationNode == null || translationNode.Text == null)
                    {
                        continue;
                    }

                    var translationValues = translationNode.Text.Split(", ");
                    foreach (var translationValue in translationValues)
                    {
                        if (lingvoInfo.Translations.Select(x => x.Text).Contains(translationValue))
                        {

                        }
                    }
                    var translation = new LexemeTranslation()
                    {
                        Text = translationNode.Text
                    };

                    var examplesNode = listItemNode.Markup.FirstOrDefault(x => x is ExamplesNode) as ExamplesNode;
                    if (examplesNode != null)
                    {
                        MapExamples(translation, examplesNode);
                    }

                    lingvoInfo.Translations.Add(translation);
                }
            }
        }
        private void MapExamples(LexemeTranslation translation, ExamplesNode examplesNode)
        {
            if (examplesNode.Items.Count < 1)
            {
                return;
            }

            foreach (var exampleItem in examplesNode.Items)
            {
                if (exampleItem is not ExampleItemNode)
                {
                    continue;
                }

                var exampleNode = exampleItem.Markup.FirstOrDefault(x => x is ExampleNode) as ExampleNode;
                if (exampleNode == null)
                {
                    continue;
                }

                for (int i = 0; i < exampleNode.Markup.Count; i += 2)
                {
                    var examples = exampleNode.Markup;
                    if (examples[i] is not TextNode
                        || examples.Count < i + 2
                        || examples[i + 1] is not TextNode
                        || examples[i].Text == null
                        || examples[i + 1].Text == null)
                    {
                        continue;
                    }

                    var lexemeExample = new LexemeExample
                    {
                        NativeExample = examples[i].Text,
                        TranslatedExample = examples[i + 1].Text
                    };
                }
            }
        }
    }
}
