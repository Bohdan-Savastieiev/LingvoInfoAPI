using LanguageStudyAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Google.Apis.Services;

namespace LanguageStudyAPI.Converters
{
    public class LingvoJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Node));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            NodeType type = jObject["Node"].ToObject<NodeType>();


            Node node;
            switch (type)
            {
                case NodeType.Comment:
                    node = new CommentNode();
                    break;
                case NodeType.Paragraph:
                    node = new ParagraphNode();
                    break;
                case NodeType.Text:
                    node = new TextNode();
                    break;
                case NodeType.List:
                    node = new ListNode();
                    break;
                case NodeType.ListItem:
                    node = new ListItemNode();
                    break;
                case NodeType.Examples:
                    node = new ExamplesNode();
                    break;
                case NodeType.ExampleItem:
                    node = new ExampleItemNode();
                    break;
                case NodeType.Example:
                    node = new ExampleNode();
                    break;
                case NodeType.CardRefs:
                    node = new CardRefsNode();
                    break;
                case NodeType.CardRefItem:
                    node = new CardRefItemNode();
                    break;
                case NodeType.CardRef:
                    node = new CardRefNode();
                    break;
                case NodeType.Transcription:
                    node = new TranscriptionNode();
                    break;
                case NodeType.Abbrev:
                    node = new AbbrevNode();
                    break;
                case NodeType.Caption:
                    node = new CaptionNode();
                    break;
                case NodeType.Sound:
                    node = new SoundNode();
                    break;
                case NodeType.Ref:
                    node = new RefNode();
                    break;
                case NodeType.Unsupported:
                    node = new UnsupportedNode();
                    break;
                default:
                    throw new ArgumentException($"Unsupported NodeType: {type}");
            }

            node.NodeType = type;

            serializer.Populate(jObject.CreateReader(), node);
            return node;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jObject = new JObject();

            Node node = (Node)value;
            jObject.Add("Node", JToken.FromObject(node.NodeType.ToString()));
            if (node.Text != null)
            {
                jObject.Add("Text", JToken.FromObject(node.Text));
            }
            else
            {
                jObject.Add("Text", null);
            }
            jObject.Add("IsOptional", JToken.FromObject(node.IsOptional));

            // Specific properties for each Node type
            switch (node)
            {
                case CommentNode commentNode:
                    jObject.Add("Markup", JToken.FromObject(commentNode.Markup));
                    break;
                case ParagraphNode paragraphNode:
                    jObject.Add("Markup", JToken.FromObject(paragraphNode.Markup));
                    break;
                case TextNode textNode:
                    jObject.Add("IsItalics", JToken.FromObject(textNode.IsItalics));
                    jObject.Add("IsAccent", JToken.FromObject(textNode.IsAccent));
                    break;
                case ListNode listNode:
                    jObject.Add("Type", JToken.FromObject(listNode.Type));
                    jObject.Add("Items", JToken.FromObject(listNode.Items));
                    break;
                case ListItemNode listItemNode:
                    jObject.Add("Markup", JToken.FromObject(listItemNode.Markup));
                    break;
                case ExamplesNode examplesNode:
                    if (examplesNode.Type != null)
                    {
                        jObject.Add("Type", JToken.FromObject(examplesNode.Type));
                    }
                    else
                    {
                        jObject.Add("Type", null);
                    }
                    jObject.Add("Items", JToken.FromObject(examplesNode.Items));
                    break;
                case ExampleItemNode exampleItemNode:
                    jObject.Add("Markup", JToken.FromObject(exampleItemNode.Markup));
                    break;
                case ExampleNode exampleNode:
                    jObject.Add("Markup", JToken.FromObject(exampleNode.Markup));
                    break;
                case CardRefsNode cardRefsNode:
                    if (cardRefsNode.Type != null)
                    {
                        jObject.Add("Type", JToken.FromObject(cardRefsNode.Type));
                    }
                    else
                    {
                        jObject.Add("Type", null);
                    }
                    jObject.Add("Items", JToken.FromObject(cardRefsNode.Items));
                    break;
                case CardRefItemNode cardRefItemNode:
                    jObject.Add("Markup", JToken.FromObject(cardRefItemNode.Markup));
                    break;
                case CardRefNode cardRefNode:
                    jObject.Add("Dictionary", JToken.FromObject(cardRefNode.Dictionary));
                    jObject.Add("ArticleId", JToken.FromObject(cardRefNode.ArticleId));
                    break;
                case AbbrevNode abbrevNode:
                    jObject.Add("FullText", JToken.FromObject(abbrevNode.FullText));
                    break;
                case SoundNode soundNode:
                    jObject.Add("FileName", JToken.FromObject(soundNode.FileName));
                    break;
                case TranscriptionNode transcription:
                case CaptionNode captionNode:
                case RefNode refNode:
                case UnsupportedNode unsupportedNode:
                    break;

                default:
                    throw new ArgumentException($"Unsupported NodeType: {node}");
            }

            jObject.WriteTo(writer);
        }
    }
}

