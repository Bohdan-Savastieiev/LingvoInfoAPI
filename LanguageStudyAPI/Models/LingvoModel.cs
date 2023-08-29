using Newtonsoft.Json;

namespace LanguageStudyAPI.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public partial class LingvoModel
    {
        [JsonProperty("Title")]
        public string Text { get; set; }

        //[JsonProperty("TitleMarkup")]
        //public Markup[] TitleMarkup { get; set; }

        //[JsonProperty("Dictionary")]
        //public string Dictionary { get; set; }

        //[JsonProperty("ArticleId")]
        //public string ArticleId { get; set; }

        [JsonProperty("Body")]
        public Body[] Body { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("Markup", NullValueHandling = NullValueHandling.Ignore)]
        public BodyMarkup[] Markup { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Text")]
        public object Text { get; set; }

        [JsonProperty("IsOptional")]
        public bool IsOptional { get; set; }

        [JsonProperty("Type", NullValueHandling = NullValueHandling.Ignore)]
        public long? Type { get; set; }

        [JsonProperty("Items", NullValueHandling = NullValueHandling.Ignore)]
        public BodyItem[] Items { get; set; }
    }

    public partial class BodyItem
    {
        [JsonProperty("Markup")]
        public PurpleMarkup[] Markup { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Text")]
        public object Text { get; set; }

        [JsonProperty("IsOptional")]
        public bool IsOptional { get; set; }
    }

    public partial class PurpleMarkup
    {
        [JsonProperty("Markup", NullValueHandling = NullValueHandling.Ignore)]
        public BodyMarkup[] Markup { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Text")]
        public object Text { get; set; }

        [JsonProperty("IsOptional")]
        public bool IsOptional { get; set; }

        [JsonProperty("Type")]
        public object Type { get; set; }

        [JsonProperty("Items", NullValueHandling = NullValueHandling.Ignore)]
        public MarkupItem[] Items { get; set; }
    }

    public partial class MarkupItem
    {
        [JsonProperty("Markup")]
        public FluffyMarkup[] Markup { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Text")]
        public object Text { get; set; }

        [JsonProperty("IsOptional")]
        public bool IsOptional { get; set; }
    }

    public partial class FluffyMarkup
    {
        [JsonProperty("Markup", NullValueHandling = NullValueHandling.Ignore)]
        public Markup[] Markup { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Text")]
        public string Text { get; set; }

        [JsonProperty("IsOptional")]
        public bool IsOptional { get; set; }

        [JsonProperty("Dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public string Dictionary { get; set; }

        [JsonProperty("ArticleId", NullValueHandling = NullValueHandling.Ignore)]
        public string ArticleId { get; set; }
    }

    public partial class Markup
    {
        [JsonProperty("IsItalics")]
        public bool IsItalics { get; set; }

        [JsonProperty("IsAccent")]
        public bool IsAccent { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Text")]
        public string Text { get; set; }

        [JsonProperty("IsOptional")]
        public bool IsOptional { get; set; }
    }

    public partial class BodyMarkup
    {
        [JsonProperty("FullText", NullValueHandling = NullValueHandling.Ignore)]
        public string FullText { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Text")]
        public string Text { get; set; }

        [JsonProperty("IsOptional")]
        public bool IsOptional { get; set; }

        [JsonProperty("IsItalics", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsItalics { get; set; }

        [JsonProperty("IsAccent", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAccent { get; set; }

        [JsonProperty("Markup", NullValueHandling = NullValueHandling.Ignore)]
        public Markup[] Markup { get; set; }

        [JsonProperty("FileName", NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; }
    }


}
