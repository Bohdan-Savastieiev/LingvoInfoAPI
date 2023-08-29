using Newtonsoft.Json;

namespace LanguageStudyAPI.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class AudioLink
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }
    }

    public class Example
    {
        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("dst")]
        public string Dst { get; set; }
    }

    public class Lemma
    {
        [JsonProperty("featured")]
        public bool Featured { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("pos")]
        public string Pos { get; set; }

        [JsonProperty("forms")]
        public List<object> Forms { get; set; }

        [JsonProperty("grammar_info")]
        public object GrammarInfo { get; set; }

        [JsonProperty("audio_links")]
        public List<AudioLink> AudioLinks { get; set; }

        [JsonProperty("translations")]
        public List<Translation> Translations { get; set; }
    }

    public class Translation
    {
        [JsonProperty("featured")]
        public bool Featured { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("pos")]
        public string Pos { get; set; }

        [JsonProperty("audio_links")]
        public object AudioLinks { get; set; }

        [JsonProperty("examples")]
        public List<Example> Examples { get; set; }

        [JsonProperty("usage_frequency")]
        public object UsageFrequency { get; set; }
    }


}
