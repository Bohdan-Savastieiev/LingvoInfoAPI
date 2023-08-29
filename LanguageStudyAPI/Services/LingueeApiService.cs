using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LanguageStudyAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace LanguageStudyAPI.Services
{
    public class LingueeApiService
    {
        private readonly HttpClient _httpClient;

        public LingueeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TranslateWordAsync(string query, string src, string dst)
        {
            string requestUrl = $"translations/?query={query}&src={src}&dst={dst}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                //List<Lemma> lemmas = JsonConvert.DeserializeObject<List<Lemma>>(responseBody);
                return responseBody;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
