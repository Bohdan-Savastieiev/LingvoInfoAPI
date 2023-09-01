using LanguageStudyAPI.Converters;
using Newtonsoft.Json;

namespace LanguageStudyAPI.Models
{
    public enum NodeType
    {
        Comment = 0,
        Paragraph = 1,
        Text = 2,
        List = 3,
        ListItem = 4,
        Examples = 5,
        ExampleItem = 6,
        Example = 7,
        CardRefs = 8,
        CardRefItem = 9,
        CardRef = 10,
        Transcription = 11,
        Abbrev = 12,
        Caption = 13,
        Sound = 14,
        Ref = 15,
        Unsupported = 16
    }
    public class ArticleModel
    {
        public string Title { get; set; }
        public List<Node> TitleMarkup { get; set; }
        public string Dictionary { get; set; }
        public string ArticleId { get; set; }
        public List<Node> Body { get; set; }
    }

    [JsonConverter(typeof(LingvoJsonConverter))]
    public class Node
    {
        // Node
        public NodeType NodeType { get; set; }
        public string Text { get; set; }
        public bool IsOptional { get; set; }
    }

    public class CommentNode : Node
    {
        public List<TextNode> Markup;
    }
    public class ParagraphNode : Node
    {
        public List<Node> Markup;
    }
    public class TextNode : Node
    {
        public bool IsItalics { get; set; }
        public bool IsAccent { get; set; }
    }
    public class ListNode : Node
    {
        public int? Type { get; set; }
        public List<ListItemNode> Items { get; set; }
    }
    public class ListItemNode : Node
    {
        public List<Node> Markup { get; set; }
    }
    public class ExamplesNode : Node
    {
        public List<ExampleItemNode> Examples { get; set; }
    }
    public class ExampleItemNode : Node
    {
        public List<Node> Markup { get; set; }
    }
    public class ExampleNode : Node
    {
        public List<TextNode> Markup { get; set; }
    }
    public class CardRefsNode : Node
    {
        public int? Type { get; set; }
        public List<CardRefItemNode> Markup { get; set; }
    }
    public class CardRefItemNode : Node
    {
        public List<CardRefNode> Markup { get; set; }
    }
    public class CardRefNode : Node
    {
        public string Dictionary { get; set; }
        public string ArticleId { get; set; }
    }
    public class TranscriptionNode : Node { }
    public class AbbrevNode : Node
    {
        public string FullText { get; set; }
    }
    public class CaptionNode : Node { }
    public class SoundNode : Node
    {
        public string FileName { get; set; }
    }  
    public class RefNode : Node { }
    public class UnsupportedNode : Node { }

    // These nodes contain no additional properties
    // NodeTypes:
    //    Transcription = 11,
    //    Caption = 13,
    //    Ref = 15,
    //    Unsupported = 16
    // public class EmptyNode : Node { }

    //public enum UniqueNodeType
    //{
    //    Text = 2,
    //    List = 3,
    //    Examples = 5,
    //    CardRefs = 8,
    //    CardRefItem = 9,
    //    CardRef = 10,
    //    Abbrev = 12,
    //    Sound = 14,
    //}
    //public enum ContainsNodesNodeType
    //{
    //    Paragraph = 1,
    //    ListItem = 4,
    //    ExampleItem = 6
    //}
    //public enum ContainsTextsNodeType
    //{
    //    Comment = 0,
    //    Example = 7,
    //}
    //public enum ContainsNothingNodeType
    //{
    //    Transcription = 11,
    //    Caption = 13,
    //    Ref = 15,
    //    Unsupported = 16
    //}
}