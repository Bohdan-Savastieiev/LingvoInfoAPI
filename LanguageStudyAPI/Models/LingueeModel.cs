namespace LanguageStudyAPI.Models
{
    public class LingueeModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        public class AudioLink
        {
            public string url { get; set; }
            public string lang { get; set; }
        }

        public class Example
        {
            public string src { get; set; }
            public string dst { get; set; }
        }

        public class Root
        {
            public bool featured { get; set; }
            public string text { get; set; }
            public string pos { get; set; }
            public List<object> forms { get; set; }
            public object grammarInfo { get; set; }
            public List<AudioLink> audioLinks { get; set; }
            public List<Translation> translations { get; set; }
        }

        public class Translation
        {
            public bool featured { get; set; }
            public string text { get; set; }
            public string pos { get; set; }
            public object audioLinks { get; set; }
            public List<Example> examples { get; set; }
            public object usageFrequency { get; set; }
        }

    }
}
