# LingvoInfoAPI (in progress)
## API Description
LingvoInfoAPI is Web API developed using .NET 7.0. Its primary purpose is to provide users with comprehensive word translations from various languages. To achieve this, our API seamlessly integrates with three renowned translation APIs: **LingvoAPI**, **LingueeAPI**, and **GoogleTranslationAPI**. By accessing these APIs, LingvoInfoAPI gathers a wealth of translation data, processes it, and then consolidates the information into a specific model. 

In case the data is not received, the program passes the request to the LingueeAPI. In case the information also has not been found, the client receive one simple translation of the lexeme provided from GoogleTranslationAPI. This model is subsequently serialized into a JSON format and made available through the Translations endpoint.

Sample Request:
```http
GET /Translations?text=example&srcLang=en&dstLang=ru
```

Sample Response:
```json
{
  "lemma": "example",
  "transcription": "ɪg'zɑːmpl",
  "audioLink": "https://www.linguee.com/mp3/EN_US/1a/1a79a4d60de6718e8e5b326e338ae533-101.mp3"
  "translations": [
    {
      "text": "пример, иллюстрация, типичный случай, аналогичный случай",
      "examples": [
        {
          "native": "to cite / give / provide an example "
          "translated": "— приводить пример"    
        }
      ]
    },
    {
      "text": "урок, назидание; предостережение",
      "examples": [
        {
          "native": "Let these unhappy examples be a warning to others. ",
          "translated": "— Пусть эти прискорбные уроки послужат предостережением другим."
        }
      ]
    }
  ],
"wordForms": [
    {
      "text": "example"
    },
    {
      "text": "examples"
    },
    {
      "text": "example's"
    },
    {
      "text": "examples'"
    }
  ]
}
```
