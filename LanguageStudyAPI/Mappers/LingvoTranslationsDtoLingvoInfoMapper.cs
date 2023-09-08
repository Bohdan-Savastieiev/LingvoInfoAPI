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
        private void MapTranslations(LingvoInfo lingvoInfo, List<LingvoTranslationsDto> dtoObjs)
        {

            foreach (var dtoObj in dtoObjs)
            {
                var listNode = dtoObj.Body.OfType<ListNode>().FirstOrDefault();
                if (listNode == null)
                {
                    MapTranslationsAndExamplesIfListNodeIsNull(lingvoInfo, dtoObj);
                }
                else
                {
                    MapTranslationsAndExamplesFromListNode(lingvoInfo, listNode);
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

                    translation.Examples.Add(lexemeExample);
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

        private void MapTranslationsAndExamplesIfListNodeIsNull(LingvoInfo lingvoInfo, LingvoTranslationsDto dtoObj)
        {
            var translationNode = dtoObj.Body
                .OfType<ParagraphNode>()
                .FirstOrDefault(paragraph => paragraph.Markup.FirstOrDefault() is TextNode)?
                .Markup.OfType<TextNode>().FirstOrDefault();
            var examplesNode = dtoObj.Body
                .OfType<ExamplesNode>().FirstOrDefault();

            AddTranslationAndExamples(lingvoInfo, translationNode, examplesNode);
        }

        private void MapTranslationsAndExamplesFromListNode(LingvoInfo lingvoInfo, ListNode paragraphNode)
        {
            foreach (var listItemNode in paragraphNode.Items)
            {
                var translationNode = listItemNode.Markup
                    .OfType<ParagraphNode>()
                    .Where(x => x.Markup.Count == 1)
                    .FirstOrDefault()?
                    .Markup.OfType<TextNode>().FirstOrDefault();
                var examplesNode = listItemNode.Markup
                    .OfType<ExamplesNode>().FirstOrDefault();

                AddTranslationAndExamples(lingvoInfo, translationNode, examplesNode);
            }
        }
        private void AddTranslationAndExamples(LingvoInfo lingvoInfo, TextNode? translationNode, ExamplesNode? examplesNode)
        {
            if (translationNode != null && !translationNode.Text.IsNullOrEmpty())
            {
                var translation = new LexemeTranslation()
                {
                    Text = translationNode.Text
                };

                if (examplesNode != null)
                {
                    MapExamples(translation, examplesNode);
                }

                lingvoInfo.Translations.Add(translation);
            }
        }

        // todo ValidateTranslations();
        //var translationValues = translationNode.Text.Split("; ");
        //foreach (var translationValue in translationValues)
        //{
        //    if (lingvoInfo.Translations.Select(x => x.Text).Contains(translationValue))
        //    {

        //    }
        //}
    }
}
