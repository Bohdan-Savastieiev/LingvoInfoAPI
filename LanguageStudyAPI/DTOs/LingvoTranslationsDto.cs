using LanguageStudyAPI.Converters;
using Newtonsoft.Json;

namespace LanguageStudyAPI.Models.Lingvo;

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
public class LingvoTranslationsDto
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
    public string? Text { get; set; }
    public bool IsOptional { get; set; }
}

public class CommentNode : Node
{
    public List<Node> Markup;
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
    public int? Type { get; set; }
    public List<ExampleItemNode> Items { get; set; }
}
public class ExampleItemNode : Node
{
    public List<Node> Markup { get; set; }
}
public class ExampleNode : Node
{
    public List<Node> Markup { get; set; }
}
public class CardRefsNode : Node
{
    public int? Type { get; set; }
    public List<CardRefItemNode> Items { get; set; }
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