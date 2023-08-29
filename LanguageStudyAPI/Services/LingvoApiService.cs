using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using LanguageStudyAPI.Models;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace LanguageStudyAPI.Services
{
    public class LingvoApiService : ILingvoApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        //private string _apiKey;
        //private string? _bearerToken;

        public LingvoApiService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;

            //var apiKey = configuration["LingvoApi:ApiKey"];
            //if (string.IsNullOrEmpty(apiKey))
            //{
            //    throw new InvalidOperationException("Lingvo Api Key has not been received from the configuration.");
            //}
            //_apiKey = apiKey;
        }

        //public async Task<bool> AuthenticateAsync()
        //{
        //    var client = _clientFactory.CreateClient("LingvoApi");

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _apiKey);

        //    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1.1/authenticate");

        //    var response = await client.SendAsync(request);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        _bearerToken = await response.Content.ReadAsStringAsync();

        //        return true;
        //    }

        //    return false;
        //}

        //public async Task ValidateToken()
        //{
        //    if (!IsTokenValid())
        //    {
        //        bool isAuthenticated = await AuthenticateAsync();
        //        if (!isAuthenticated)
        //        {
        //            throw new InvalidOperationException("Authentication has not been completed.");
        //        }
        //    }
        //}
        public async Task<string> GetTranslationAsync(string text, string srcLang, string dstLang, bool isCaseSensitive)
        {
            //await ValidateToken();

            var client = _clientFactory.CreateClient("LingvoApi");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            string requestUrl = $"api/v1/Translation?text={text}&srcLang={srcLang}&dstLang={dstLang}&isCaseSensitive={isCaseSensitive}";
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error calling Lingvo API: {response.ReasonPhrase}");
            }
            //try
            //{
            //    _httpClient.DefaultRequestHeaders.Clear();
            //    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_bearerToken}");

            //    string translationUrl = $"{_apiTranslationUrl}?text={text}&srcLang={srcLang}&dstLang={dstLang}&isCaseSensitive={isCaseSensitive}";

            //    HttpResponseMessage translationResponse = await _httpClient.GetAsync(translationUrl);

            //    if (translationResponse.IsSuccessStatusCode)
            //    {
            //        string translationResult = await translationResponse.Content.ReadAsStringAsync();
            //        var jArray = JArray.Parse(translationResult);
            //        //FlattenMarkupNodes(jArray);
            //        var examples = FindAllExampleNodes(jArray);
            //        var lexemeExamplesList = ConvertToLexemeExamples(examples);

            //        JArray resultArray = new JArray();

            //        foreach (var token in examples)
            //        {
            //            resultArray.Add(token.DeepClone()); // добавляем копию токена в массив
            //        }
            //        return resultArray.ToString();

            //        var strExs = new List<string>();
            //        foreach(var example in examples)
            //        {
            //            strExs.Add(example.ToString());
            //        }

            //        translationResult = jArray.ToString();
            //        var rootObjects = JsonConvert.DeserializeObject<List<LingvoModel>>(translationResult);
            //        //var result = ConvertToLexemeModels(rootObjects);
            //        return translationResult;
            //    }
            //    else
            //    {
            //        throw new ArgumentException();
            //        //return $"Translation request failed. Status code: {translationResponse.StatusCode}";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //    //return $"An error occurred: {ex.Message}";
            //}
        }
        public async Task<List<LexemeExample>> GetExamplesFromTranslationAsync(string text, string srcLang, string dstLang, bool isCaseSensitive)
        {
            string response = await GetTranslationAsync(text, srcLang, dstLang, isCaseSensitive);
            var responseJArray = JArray.Parse(response);
            var examples = FindAllExampleNodes(responseJArray);
            return ConvertToLexemeExamples(examples);
        }

        public async Task<string> GetSoundAsync(string dictionaryName, string fileName)
        {
            // ValidateToken();

            var client = _clientFactory.CreateClient("LingvoApi");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            string requestUrl = $"api/v1/Translation?dictionaryName={dictionaryName}&fileName={fileName}";
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error calling Lingvo API: {response.ReasonPhrase}");
            }
        }

        public static List<LexemeExample> ConvertToLexemeExamples(List<JToken> examples)
        {
            List<LexemeExample> lexemeExamples = new List<LexemeExample>();

            foreach (var example in examples)
            {
                var markupArray = example["Markup"] as JArray;
                if (markupArray != null && markupArray.Count >= 2)
                {
                    var texts = markupArray.Where(x => x["Node"]?.ToString() == "Text").Select(x => x["Text"]?.ToString() ?? "");
                    var native = string.Join("", texts.TakeWhile(x => !x.Contains("—")));
                    var trans = string.Join("", texts.SkipWhile(x => !x.Contains("—")));

                    lexemeExamples.Add(new LexemeExample
                    {
                        NativeExample = native,
                        TranslatedExample = trans
                    });
                }
            }

            return lexemeExamples;
        }
        

        //private bool IsTokenValid()
        //{
        //    if (string.IsNullOrEmpty(_bearerToken))
        //    {
        //        return false;
        //    }
        //    try
        //    {
        //        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //        JwtSecurityToken token = tokenHandler.ReadJwtToken(_bearerToken);

        //        if (token.ValidTo <= DateTime.UtcNow.AddMinutes(1))
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public List<LexemeModel> ConvertToLexemeModels(string response)
        {
            List<LexemeModel> lexemeModels = new List<LexemeModel>();

            var lingvoModels = JsonConvert.DeserializeObject<List<LingvoModel>>(response)
                ?? throw new ArgumentNullException("lingvoModels");

            foreach (var lingvo in lingvoModels)
            {
                LexemeModel lexeme = new LexemeModel
                {
                    Text = lingvo.Text,
                    Examples = new List<LexemeExample>()
                };

                foreach (var body in lingvo.Body)
                {
                    // Извлечение перевода слова
                    if (body.Node == "Paragraph" && body.Markup != null)
                    {
                        foreach (var markup in body.Markup)
                        {
                            if (markup.Node == "Text")
                            {
                                lexeme.Translation = new LexemeTranslation { Text = markup.Text };
                            }
                        }
                    }

                    // Извлечение примеров использования
                    if (body.Node == "Examples" && body.Items != null)
                    {
                        foreach (var item in body.Items)
                        {
                            if (item.Markup != null)
                            {
                                foreach (var exampleMarkup in item.Markup)
                                {
                                    if (exampleMarkup.Node == "Example" && exampleMarkup.Markup != null)
                                    {
                                        var example = new LexemeExample
                                        {
                                            NativeExample = exampleMarkup.Markup[0]?.Text,
                                            TranslatedExample = exampleMarkup.Markup[1]?.Text
                                        };

                                        lexeme.Examples.Add(example);
                                    }
                                }
                            }
                        }
                    }
                }

                lexemeModels.Add(lexeme);
            }

            return lexemeModels;
        }

        public static void RemovePropertiesExcept(JObject obj, string prop)
        {
            var propertiesToRemove = obj.Properties()
                                         .Where(p => p.Name != prop)
                                         .ToList();

            foreach (var property in propertiesToRemove)
            {
                property.Remove();
            }
        }


        public static List<JToken> FindAllExampleNodes(JToken token)
        {
            List<JToken> results = new List<JToken>();

            // Если текущий токен является объектом JObject и содержит "Node": "Example"
            if (token is JObject jObject && jObject["Node"]?.ToString() == "Example")
            {
                var obj = (JObject)jObject.DeepClone();
                RemovePropertiesExcept(obj, "Markup");
                results.Add(obj);
                //results.Add(jObject);
            }

            // Если у элемента есть дочерние элементы и это не JValue, проверьте их рекурсивно
            if (!(token is JValue))
            {
                foreach (var child in token.Children())
                {
                    results.AddRange(FindAllExampleNodes(child));
                }
            }

            return results;
        }


        public static void FlattenMarkupNodes(JToken parentToken)
        {
            if (parentToken is JContainer container)
            {
                var markupProperties = container.DescendantsAndSelf()
                                                .OfType<JProperty>()
                                                .Where(p => p.Name == "Markup")
                                                .ToList();

                foreach (var markupProperty in markupProperties)
                {
                    // Если "Markup" это массив
                    if (markupProperty.Value is JArray markupArray)
                    {
                        JToken previous = markupProperty;
                        foreach (var markupChild in markupArray.Children().ToList())
                        {
                            markupChild.Remove();
                            if (markupChild is JObject childObject)
                            {
                                foreach (var childProperty in childObject.Properties().ToList())
                                {
                                    childProperty.Remove();
                                    ((JObject)markupProperty.Parent).Add(childProperty.Name, childProperty.Value);
                                }
                            }
                            else
                            {
                                previous.AddAfterSelf(markupChild);
                                previous = markupChild;
                            }
                        }
                    }
                    // Если "Markup" это объект
                    else if (markupProperty.Value is JObject markupObject)
                    {
                        foreach (var markupChildProperty in markupObject.Properties().ToList())
                        {
                            markupChildProperty.Remove();
                            ((JObject)markupProperty.Parent).Add(markupChildProperty.Name, markupChildProperty.Value);
                        }
                    }

                    // Удаляем "Markup"
                    markupProperty.Remove();
                }
            }
        }

        //public async Task<string> GetSearchAsync(string text, int srcLang, int dstLang, 
        //    int searchZone, int startIndex, int pageSize)
        //{
        //    if (IsTokenInvalidOrExpired())
        //    {
        //        bool isAuthenticated = await AuthenticateAsync();
        //        if (!isAuthenticated)
        //        {
        //            throw new ArgumentException();
        //            //return "Authentication failed.";
        //        }
        //    }

        //    try
        //    {
        //        _httpClient.DefaultRequestHeaders.Clear();
        //        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_bearerToken}");

        //        string searchUrl = $"{_apiSearchUrl}?text={text}&srcLang={srcLang}&dstLang={dstLang}&searchZone={searchZone}&startIndex={startIndex}&pageSize={pageSize}";

        //        HttpResponseMessage translationResponse = await _httpClient.GetAsync(searchUrl);

        //        if (translationResponse.IsSuccessStatusCode)
        //        {
        //            string translationResult = await translationResponse.Content.ReadAsStringAsync();
        //            //var rootObjects = JsonConvert.DeserializeObject<List<LingvoModel>>(translationResult);
        //            //var result = ConvertToLexemeModels(rootObjects);
        //            return translationResult;
        //        }
        //        else
        //        {
        //            throw new ArgumentException();
        //            //return $"Translation request failed. Status code: {translationResponse.StatusCode}";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //        //return $"An error occurred: {ex.Message}";
        //    }
        //}


        //public async Task<string> GetMinicardAsync(string text, string srcLang, string dstLang)
        //{
        //    if (IsTokenInvalidOrExpired())
        //    {
        //        bool isAuthenticated = await AuthenticateAsync();
        //        if (!isAuthenticated)
        //        {
        //            return "Authentication failed.";
        //        }
        //    }

        //    try
        //    {
        //        _httpClient.DefaultRequestHeaders.Clear();
        //        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_bearerToken}");

        //        string minicardUrl = $"{_apiMinicardUrl}?text={text}&srcLang={srcLang}&dstLang={dstLang}";

        //        HttpResponseMessage translationResponse = await _httpClient.GetAsync(minicardUrl);

        //        if (translationResponse.IsSuccessStatusCode)
        //        {
        //            string translationResult = await translationResponse.Content.ReadAsStringAsync();
        //            return translationResult;
        //        }
        //        else
        //        {
        //            return $"Translation request failed. Status code: {translationResponse.StatusCode}";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"An error occurred: {ex.Message}";
        //    }
        //}




    }
}
