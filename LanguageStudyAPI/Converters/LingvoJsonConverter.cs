using LanguageStudyAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Google.Apis.Services;
using LanguageStudyAPI.Models.Lingvo;
using LingvoInfoAPI.Factories;

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

            Node node = CreateNode(type);
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

        public Node CreateNode(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.Comment:
                    return new CommentNode();
                case NodeType.Paragraph:
                    return new ParagraphNode();
                case NodeType.Text:
                    return new TextNode();
                case NodeType.List:
                    return new ListNode();
                case NodeType.ListItem:
                    return new ListItemNode();
                case NodeType.Examples:
                    return new ExamplesNode();
                case NodeType.ExampleItem:
                    return new ExampleItemNode();
                case NodeType.Example:
                    return new ExampleNode();
                case NodeType.CardRefs:
                    return new CardRefsNode();
                case NodeType.CardRefItem:
                    return new CardRefItemNode();
                case NodeType.CardRef:
                    return new CardRefNode();
                case NodeType.Transcription:
                    return new TranscriptionNode();
                case NodeType.Abbrev:
                    return new AbbrevNode();
                case NodeType.Caption:
                    return new CaptionNode();
                case NodeType.Sound:
                    return new SoundNode();
                case NodeType.Ref:
                    return new RefNode();
                case NodeType.Unsupported:
                    return new UnsupportedNode();
                default:
                    throw new ArgumentException($"Unsupported NodeType: {nodeType}");
            }
        }
    }
}

