using LanguageStudyAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

            serializer.Populate(jObject.CreateReader(), node);
            return node;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
