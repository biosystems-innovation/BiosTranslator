using Domain;
using Domain.Models;
using Newtonsoft.Json;

namespace BusinessTranslator;

/// <summary>
/// Get list of supported languages for translation
/// Documentation: https://docs.azure.cn/en-us/ai-services/translator/text-translation/reference/v3/languages
/// </summary>
public class OptionsTranslationBusiness : IOptionsTranslationBusiness
{
    private const string Route = "languages?api-version=3.0&scope=translation";

    public async Task<IEnumerable<SupportedLanguageDomain>> GetSupportedLanguages(string endPoint, string key)
    {
        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            // Build the request.
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(endPoint + Route);
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            // location required if you're using a multi-service or regional (not global) resource.
            request.Headers.Add("Ocp-Apim-Subscription-Region", TranslatorConstants.LocationWestEurope);

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            // Read response as a string.
            string result = await response.Content.ReadAsStringAsync();

            // Parse the JSON correctly
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(result);
        
            if (jsonObject?.translation != null)
            {
                var languages = new List<SupportedLanguageDomain>();
                foreach (var property in jsonObject.translation)
                {
                    //"af":{"name":"Afrikaans","nativeName":"Afrikaans","dir":"ltr"}
                    var languageCode = property.Name;
                    var languageDetails = property.Value;
                    SupportedLanguageDomain supported = new ()
                    {
                        Code = languageCode, //language code (e.g., "es", "en", "fr")
                        Name = languageDetails.name?.ToString() ?? string.Empty //language name (e.g., "Spanish", "English", "French")
                    };
                    languages.Add(supported); 
                }
                return languages;
            }

            return [];
        }
    }
}