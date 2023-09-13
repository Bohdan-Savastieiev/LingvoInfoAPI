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
  "lemma": "translation",
  "transcription": "trænz'leɪʃ(ə)n",
  "translations": [
    {
      "text": "перевод",
      "examples": [
        {
          "nativeExample": "authorized translation ",
          "translatedExample": "— авторизованный перевод"
        },
        {
          "nativeExample": "close / literal / word-for-word translation ",
          "translatedExample": "— дословный перевод"
        },
        {
          "nativeExample": "free / loose translation ",
          "translatedExample": "— свободный, вольный перевод"
        },
        {
          "nativeExample": "loan translation ",
          "translatedExample": "— калька"
        },
        {
          "nativeExample": "machine translation ",
          "translatedExample": "— машинный перевод, автоматический перевод"
        },
        {
          "nativeExample": "simultaneous translation ",
          "translatedExample": "— синхронный перевод"
        },
        {
          "nativeExample": "translation from Russian into English ",
          "translatedExample": "— перевод с русского на английский"
        },
        {
          "nativeExample": "in translation ",
          "translatedExample": "— в переводе"
        },
        {
          "nativeExample": "to read a book in translation ",
          "translatedExample": "— читать книгу в переводе"
        },
        {
          "nativeExample": "The poem is very effective in free / in a free translation. ",
          "translatedExample": "— Стихотворение звучит очень красиво в вольном переводе."
        }
      ],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": [
        {
          "text": "translation of motion",
          "scope": "Engineering"
        },
        {
          "text": "beam translation",
          "scope": "Engineering"
        },
        {
          "text": "computer-aided translation",
          "scope": "Engineering"
        },
        {
          "text": "kinematic translation",
          "scope": "Engineering"
        },
        {
          "text": "machine-aided translation",
          "scope": "Engineering"
        },
        {
          "text": "program translation",
          "scope": "Engineering"
        }
      ]
    },
    {
      "text": "преобразование, изменение",
      "examples": [],
      "synonyms": [
        {
          "text": "transformation"
        },
        {
          "text": "alteration"
        },
        {
          "text": "change"
        },
        {
          "text": "renovation"
        }
      ],
      "antonyms": [],
      "derivedLexemes": []
    },
    {
      "text": "трансляция",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": [
        {
          "text": "genetic translation",
          "scope": "Biology"
        },
        {
          "text": "hybrid-arrested translation",
          "scope": "Biology"
        },
        {
          "text": "hybrid-released translation",
          "scope": "Biology"
        },
        {
          "text": "nick translation",
          "scope": "Biology"
        }
      ]
    },
    {
      "text": "пересчет",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": [
        {
          "text": "machine translation",
          "scope": "Psychology"
        },
        {
          "text": "verbal translation",
          "scope": "Psychology"
        }
      ]
    },
    {
      "text": "преобразование",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": []
    },
    {
      "text": "пересчёт",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": []
    },
    {
      "text": "перемещение, сдвиг",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": []
    },
    {
      "text": "поступательное движение",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": [
        {
          "text": "translations among multiple languages",
          "scope": "Computer"
        },
        {
          "text": "address translation",
          "scope": "Computer"
        },
        {
          "text": "algorithmic translation",
          "scope": "Computer"
        },
        {
          "text": "algorithm translation",
          "scope": "Computer"
        },
        {
          "text": "code translation",
          "scope": "Computer"
        },
        {
          "text": "data translation",
          "scope": "Computer"
        },
        {
          "text": "dynamic address translation",
          "scope": "Computer"
        },
        {
          "text": "electronic translation",
          "scope": "Computer"
        },
        {
          "text": "enumerated type translation",
          "scope": "Computer"
        },
        {
          "text": "formula translation",
          "scope": "Computer"
        },
        {
          "text": "functional translation",
          "scope": "Computer"
        },
        {
          "text": "human translation",
          "scope": "Computer"
        },
        {
          "text": "human-aided machine translation",
          "scope": "Computer"
        },
        {
          "text": "interlinear translation",
          "scope": "Computer"
        },
        {
          "text": "interpretive translation",
          "scope": "Computer"
        },
        {
          "text": "language translation",
          "scope": "Computer"
        },
        {
          "text": "literal translation",
          "scope": "Computer"
        },
        {
          "text": "machine translation",
          "scope": "Computer"
        },
        {
          "text": "mechanical translation",
          "scope": "Computer"
        },
        {
          "text": "machine-aided human translation",
          "scope": "Computer"
        },
        {
          "text": "one-for-one translation",
          "scope": "Computer"
        },
        {
          "text": "one-to-one translation",
          "scope": "Computer"
        },
        {
          "text": "sentence-for-sentence translation",
          "scope": "Computer"
        },
        {
          "text": "several-for-one translation",
          "scope": "Computer"
        },
        {
          "text": "simultaneous translation",
          "scope": "Computer"
        },
        {
          "text": "word-by-word translation",
          "scope": "Computer"
        },
        {
          "text": "word-for-word translation",
          "scope": "Computer"
        }
      ]
    },
    {
      "text": "сдвиг, перенос",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": []
    },
    {
      "text": "преобразование, трансформация",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": []
    },
    {
      "text": "транспонирование",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": [
        {
          "text": "address translation",
          "scope": "Telecoms"
        },
        {
          "text": "code translation",
          "scope": "Telecoms"
        },
        {
          "text": "direct translation",
          "scope": "Telecoms"
        },
        {
          "text": "frequency translation",
          "scope": "Telecoms"
        },
        {
          "text": "image translation",
          "scope": "Telecoms"
        },
        {
          "text": "number characters translation",
          "scope": "Telecoms"
        },
        {
          "text": "radio translation",
          "scope": "Telecoms"
        },
        {
          "text": "telephone translation",
          "scope": "Telecoms"
        }
      ]
    },
    {
      "text": "прямолинейное перемещение, равномерное прямолинейное перемещение; поступательное движение, поступательное передвижение; перенос",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": []
    },
    {
      "text": "преобразование; пересчёт",
      "examples": [],
      "synonyms": [],
      "antonyms": [],
      "derivedLexemes": []
    }
  ],
  "wordForms": [
    {
      "text": "translation"
    },
    {
      "text": "translations"
    },
    {
      "text": "translation's"
    },
    {
      "text": "translations'"
    }
  ],
  "sound": null
}
```
