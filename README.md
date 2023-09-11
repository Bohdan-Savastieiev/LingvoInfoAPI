# LingvoInfoAPI (in progress)
## API Description
LingvoInfoAPI is Web API developed using .NET 7.0. Its primary purpose is to provide users with comprehensive word translations from various languages. To achieve this, our API seamlessly integrates with three renowned translation APIs: **LingvoAPI**, **LingueeAPI**, and **GoogleTranslationAPI**. By accessing these APIs, LingvoInfoAPI gathers a wealth of translation data, processes it, and then consolidates the information into a specific model. 

In case the data is not received, the program passes the request to the LingueeAPI. In case the information also has not been found, the client receive one simple translation of the lexeme provided from GoogleTranslationAPI. This model is subsequently serialized into a JSON format and made available through the Translations endpoint.

#### Included languages
"zh" – Chinese (Simplified),
"de" - German,
"el" - Greek,
"en" - English,
"es" - Spanish,
"fr" - French,
"it" - Italian,
"ru" - Russian,
"uk" - Ukrainian.

#### Sample Request:
```http
GET /Translations?text=example&srcLang=en&dstLang=ru&includeSound=false
```

#### Sample Response:
```json
{
  "lemma": "monarchy",
  "transcription": "'mɔnəkɪ",
  "translations": [
    {
      "text": "монархия",
      "examples": [
        {
          "nativeExample": "to establish / set up a monarchy ",
          "translatedExample": "— установить монархию"
        },
        {
          "nativeExample": "to overthrow a monarchy ",
          "translatedExample": "— свергнуть монархию"
        }
      ],
      "synonyms": [],
      "antonyms": []
    }
  ],
  "wordForms": [
    {
      "text": "monarchy"
    },
    {
      "text": "monarchies"
    },
    {
      "text": "monarchy's"
    },
    {
      "text": "monarchies'"
    }
  ],
  "sound": null
}
```
