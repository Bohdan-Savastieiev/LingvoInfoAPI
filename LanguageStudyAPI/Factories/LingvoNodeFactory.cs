using LanguageStudyAPI.Models.Lingvo;

namespace LingvoInfoAPI.Factories
{
    public class LingvoNodeFactory : INodeFactory
    {
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
