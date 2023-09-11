using System;
using System.Linq;
using System.Collections.Generic;
using LanguageStudyAPI.Models.Lingvo;
using LanguageStudyAPI.Models;
using LingvoInfoAPI.DTOs;
using LanguageExt;
using LanguageExt.Common;

namespace LingvoInfoAPI.Mappers
{
    public class LingvoTranslationsDtoLingvoInfoMapper : ILingvoInfoMapper<LingvoTranslationsDto>
    {
        public LingvoInfo MapToLingvoInfo(List<LingvoTranslationsDto> translations)
        {
            ValidateTranslationsDtos(translations);

            var lingvoInfo = new LingvoInfo
            {
                Lemma = translations.First().Title
            };

            SetTranscription(lingvoInfo, translations);
            SetTranslations(lingvoInfo, translations);

            return lingvoInfo;
        }

        private void ValidateTranslationsDtos(List<LingvoTranslationsDto> translations)
        {
            if (translations == null || !translations.Any())
            {
                throw new ArgumentNullException(nameof(translations));
            }
        }

        private void SetTranscription(LingvoInfo lingvoInfo, List<LingvoTranslationsDto> translations)
        {
            foreach (var dtoObj in translations)
            {
                var transcriptionNode = GetTranscriptionNode(dtoObj);
                if (transcriptionNode != null && !string.IsNullOrEmpty(transcriptionNode.Text))
                {
                    lingvoInfo.Transcription = transcriptionNode.Text;
                    return;
                }
            }
        }
        private TranscriptionNode? GetTranscriptionNode(LingvoTranslationsDto translation)
        {
            var paragraphNode = translation.Body.OfType<ParagraphNode>().FirstOrDefault();
            return paragraphNode?.Markup.OfType<TranscriptionNode>().FirstOrDefault();
        }

        private void SetTranslations(LingvoInfo lingvoInfo, List<LingvoTranslationsDto> dtoObjs)
        {
            var translations = new List<LexemeTranslation>();
            foreach (var dtoObj in dtoObjs)
            {
                var bodyNodes = dtoObj.Body;
                AssignTranslations(translations, bodyNodes);
            }
            lingvoInfo.Translations = translations;
        }
        private void AssignTranslations(List<LexemeTranslation> translations, List<Node> nodes)
        {
            // JSON file contains various amount of nested lists
            if (nodes.Any(x => x is ListNode))
            {
                var listNode = nodes.OfType<ListNode>().First();
                foreach (var listItemNode in listNode.Items)
                {
                    AssignTranslations(translations, listItemNode.Markup);
                }
            }
            else
            {
                // get the Translation and it's properties: Examples, Synonyms, Antonyms
                var translation = GetTranslation(nodes);
                if (translation != null)
                {
                    AssignNewTranslation(translations, translation);
                }
            }
        }
        private LexemeTranslation? GetTranslation(List<Node> nodes)
        {
            var translations = nodes
                .OfType<ParagraphNode>()
                .FirstOrDefault(paragraph => paragraph.Markup.FirstOrDefault() is TextNode)?
                .Markup.Where(x => x is TextNode && x.Text != null && !x.IsOptional)
                .Select(x => x.Text);

            if (translations != null && !translations.Any(x => x.Contains('(') || x.StartsWith('=')))
            {
                var result = new LexemeTranslation
                {
                    Text = string.Join("", translations).Trim(),
                    Examples = GetExamples(nodes),
                    Synonyms = GetSynonyms(nodes),
                    Antonyms = GetAntonyms(nodes)
                };

                return result;
            }

            return null;
        }

        private List<Synonym> GetSynonyms(List<Node> nodes)
        {
            return GetRelatedLexemes(nodes, "Syn:").Select(text => new Synonym { Text = text }).ToList();
        }

        private List<Antonym> GetAntonyms(List<Node> nodes)
        {
            return GetRelatedLexemes(nodes, "Ant:").Select(text => new Antonym { Text = text }).ToList();
        }

        private List<string> GetRelatedLexemes(List<Node> nodes, string captionText)
        {
            var result = new List<string>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeType == NodeType.Caption
                    && nodes[i].Text == captionText
                    && i < nodes.Count - 1
                    && nodes[i + 1] is ParagraphNode lexemeNode)
                {
                    var lexemes = lexemeNode.Markup
                        .Select(x => x.Text)
                        .Where((item, index) => index % 2 == 0 && item != null)
                        .ToList();
                    result.AddRange(lexemes);
                }
            }

            return result;
        }


        private List<LexemeExample> GetExamples(List<Node> nodes)
        {
            var examples = new List<LexemeExample>();
            var examplesNode = nodes.OfType<ExamplesNode>().FirstOrDefault();
            if (examplesNode != null && examplesNode.Items.Count != 0)
            {
                foreach (var exampleItem in examplesNode.Items)
                {
                    if (!(exampleItem is ExampleItemNode))
                    {
                        continue;
                    }

                    var exampleNode = exampleItem.Markup.OfType<ExampleNode>().FirstOrDefault();
                    if (exampleNode == null)
                    {
                        continue;
                    }
                    var textStrings = exampleNode.Markup
                        .Where(x => x is TextNode && x.Text != null)
                        .Select(x => x.Text);
                    var nativeExampleStrings = textStrings
                        .TakeWhile(x => !x.StartsWith('—'))
                        .ToList();
                    var translatedExampleStrings = textStrings
                        .SkipWhile(x => !x.StartsWith('—'))
                        .ToList();
                    var nativeExample = string.Concat(nativeExampleStrings);
                    var translatedExample = string.Concat(translatedExampleStrings);

                    var lexemeExample = new LexemeExample
                    {
                        NativeExample = nativeExample,
                        TranslatedExample = translatedExample
                    };

                    examples.Add(lexemeExample);
                }
            }

            return examples;
        }
        private void AssignNewTranslation(List<LexemeTranslation> translations, LexemeTranslation translation)
        {
            var equalTranslation = translations.FirstOrDefault(x => string.Equals
                    (x.Text, translation.Text, StringComparison.InvariantCultureIgnoreCase));
            if (equalTranslation == null)
            {
                translations.Add(translation);
            }
            else
            {
                equalTranslation.Examples.AddRange(translation.Examples);
            }
        }
    }
}
