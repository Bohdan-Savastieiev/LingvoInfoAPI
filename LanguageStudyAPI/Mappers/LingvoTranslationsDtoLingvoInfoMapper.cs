using System;
using System.Linq;
using System.Collections.Generic;
using LanguageStudyAPI.Models.Lingvo;
using LanguageStudyAPI.Models;
using LingvoInfoAPI.DTOs;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Bson;

namespace LingvoInfoAPI.Mappers
{
    public class LingvoTranslationsDtoLingvoInfoMapper : ILingvoInfoMapper<LingvoTranslationsDto>
    {
        public LingvoInfo MapToLingvoInfo(List<LingvoTranslationsDto> translationsDtos)
        {
            ValidateTranslationsDtos(translationsDtos);

            var lingvoInfo = new LingvoInfo
            {
                Lemma = translationsDtos.First().Title
            };

            SetTranscription(lingvoInfo, translationsDtos);
            SetTranslations(lingvoInfo, translationsDtos);

            return lingvoInfo;
        }

        private void ValidateTranslationsDtos(List<LingvoTranslationsDto> translationsDtos)
        {
            if (translationsDtos == null || !translationsDtos.Any())
            {
                throw new ArgumentNullException(nameof(translationsDtos));
            }
        }

        // Transcriptions
        private void SetTranscription(LingvoInfo lingvoInfo, List<LingvoTranslationsDto> translationsDtos)
        {
            foreach (var dtoObj in translationsDtos)
            {
                var transcriptionNode = GetTranscriptionNode(dtoObj);
                if (transcriptionNode != null && !string.IsNullOrEmpty(transcriptionNode.Text))
                {
                    lingvoInfo.Transcription = transcriptionNode.Text;
                    return;
                }
            }
        }
        private TranscriptionNode? GetTranscriptionNode(LingvoTranslationsDto translationsDto)
        {
            var paragraphNode = translationsDto.Body.OfType<ParagraphNode>().FirstOrDefault();
            return paragraphNode?.Markup.OfType<TranscriptionNode>().FirstOrDefault();
        }

        // Translations and all included
        private void SetTranslations(LingvoInfo lingvoInfo, List<LingvoTranslationsDto> dtoObjs)
        {
            var translations = new List<LexemeTranslation>();
            foreach (var dtoObj in dtoObjs)
            {
                AssignTranslations(translations, dtoObj.Body);
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
                    Antonyms = GetAntonyms(nodes),
                    DerivedLexemes = GetDerivedLexemes(nodes)
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
        private List<DerivedLexeme> GetDerivedLexemes(List<Node> nodes)
        {
            var derivedLexemes = new List<DerivedLexeme>();
            var cardRefsNode = nodes.OfType<CardRefsNode>().FirstOrDefault();
            if (cardRefsNode != null && cardRefsNode.Items.Count != 0)
            {
                foreach (var cardRefItem in cardRefsNode.Items)
                {
                    if (!(cardRefItem is CardRefItemNode))
                    {
                        continue;
                    }

                    var exampleNode = cardRefItem.Markup.OfType<CardRefNode>().FirstOrDefault();
                    if (exampleNode == null || exampleNode.Text.IsNullOrEmpty())
                    {
                        continue;
                    }

                    var scope = GetScopeFromDictionary(exampleNode.Dictionary);

                    var derivedLexeme = new DerivedLexeme
                    {
                        Text = exampleNode.Text,
                        Scope = scope
                    };

                    derivedLexemes.Add(derivedLexeme);
                }
            }

            return derivedLexemes;
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
        private string GetScopeFromDictionary(string dictionaryName)
        {
            var scope = dictionaryName.Split(' ')[0];
            if (scope.Contains("Lingvo"))
            {
                scope = scope.Replace("Lingvo", "");
            }

            if (scope.Contains("American"))
            {
                scope = "Universal";
            }

            return scope;
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
                equalTranslation.Synonyms.AddRange(translation.Synonyms);
                equalTranslation.Antonyms.AddRange(translation.Antonyms);
                equalTranslation.DerivedLexemes.AddRange(translation.DerivedLexemes);
            }
        }
    }
}
