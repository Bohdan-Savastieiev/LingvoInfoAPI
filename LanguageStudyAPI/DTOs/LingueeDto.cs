using Newtonsoft.Json;

namespace LingvoInfoAPI.DTOs;

public class LingueeDto
{
    [JsonProperty("featured")]
    public bool Featured { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("pos")]
    public string Pos { get; set; }

    [JsonProperty("forms")]
    public object[] Forms { get; set; }

    [JsonProperty("grammar_info")]
    public object GrammarInfo { get; set; }

    [JsonProperty("audio_links")]
    public AudioLink[] AudioLinks { get; set; }

    [JsonProperty("translations")]
    public Translation[] Translations { get; set; }
}

public class AudioLink
{
    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("lang")]
    public string Lang { get; set; }
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
    public Example[] Examples { get; set; }

    [JsonProperty("usage_frequency")]
    public string UsageFrequency { get; set; }
}

public class Example
{
    [JsonProperty("src")]
    public string Src { get; set; }

    [JsonProperty("dst")]
    public string Dst { get; set; }
}
