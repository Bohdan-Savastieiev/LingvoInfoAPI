using Newtonsoft.Json;

namespace LingvoInfoAPI.DTOs
{
    public class LingvoWordFormsDto
    {
        [JsonProperty("Lexem")]
        public string Lexem { get; set; }

        [JsonProperty("PartOfSpeech")]
        public string PartOfSpeech { get; set; }

        [JsonProperty("ParadigmJson")]
        public ParadigmJson ParadigmJson { get; set; }
    }

    public class ParadigmJson
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Grammar")]
        public string Grammar { get; set; }

        [JsonProperty("Groups")]
        public Group[] Groups { get; set; }
    }

    public class Group
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Table")]
        public Table[][] Table { get; set; }

        [JsonProperty("ColumnCount")]
        public long ColumnCount { get; set; }

        [JsonProperty("RowCount")]
        public long RowCount { get; set; }
    }

    public class Table
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Prefix")]
        public string Prefix { get; set; }

        [JsonProperty("Row")]
        public object Row { get; set; }
    }
}
