# LingvoInfoAPI
## API Description
LingvoInfoAPI is Web API developed using .NET 7.0. Its primary purpose is to provide users with comprehensive word translations from various languages. To achieve this, API integrates with several external APIs. 

#### Included languages
- "zh" – Chinese (Simplified)
- "de" – German
- "el" – Greek
- "en" – English
- "es" – Spanish
- "fr" – French
- "it" – Italian
- "ru" – Russian
- "uk" – Ukrainian

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
