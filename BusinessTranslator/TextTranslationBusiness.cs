using Domain;
using Domain.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace BusinessTranslator;

public class TextTranslationBusiness : ITextTranslationBusiness
{
    //Route scheme: "translate?api-version=3.0&from=en&to=fr&to=zu" //Explicit from (origin) language
    //Route scheme: "translate?api-version=3.0&to=fr&to=zu" //Auto detect from (origin) language
    private const string RouteBase = "translate?api-version=3.0";

    public async Task<IEnumerable<TranslationDomain>> Translate(IEnumerable<string> toTranslate, IEnumerable<string> to, string from, string endPoint, string key)
    {
        string route = BuildRoute(to, from);
        return await TranslateViaExternalApi(toTranslate, endPoint, key, route);
    }

    public async Task<IEnumerable<TranslationDomain>> Translate(IEnumerable<string> toTranslate, IEnumerable<string> to, string endPoint, string key)
    {
        string route = BuildRoute(to);
        return await TranslateViaExternalApi(toTranslate, endPoint, key, route);
    }


    #region Private

    private string BuildRoute(IEnumerable<string> to, string? from = null)
    {
        var route = RouteBase;
        if (!string.IsNullOrEmpty(from))
        {
            route += $"&from={from}";
        }

        foreach (var lang in to)
        {
            route += $"&to={lang}";
        }

        return route;
    }

    private string BuildRequestBody(IEnumerable<string> toTranslate)
    {
        var body = toTranslate.Select(t => new { Text = t }).ToArray();
        return JsonConvert.SerializeObject(body);
    }

    private async Task<IEnumerable<TranslationDomain>> TranslateViaExternalApi(IEnumerable<string> toTranslate, string endPoint, string key, string route)
    {
        string requestBody = BuildRequestBody(toTranslate);

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            // Build the request.
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(endPoint + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            // location required if you're using a multi-service or regional (not global) resource.
            request.Headers.Add("Ocp-Apim-Subscription-Region", TranslatorConstants.LocationWestEurope);

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            // Read response as a string.
            string result = await response.Content.ReadAsStringAsync();

            //[{"translations":[{"text":"De toute façon","to":"fr"},{"text":"De qualquer maneira","to":"pt"},{"text":"Comunque","to":"it"}]}]

            // Parse the JSON correctly
            var jsonArray = JsonConvert.DeserializeObject<dynamic>(result);
            if (jsonArray != null && jsonArray.Count > 0)
            {
                var translations = new List<TranslationDomain>();
                foreach (var itemResult in jsonArray)
                {
                    if (itemResult?.translations != null)
                    {
                        foreach (var translationItem in itemResult.translations)
                        {
                            var translatedText = translationItem.text?.ToString() ?? string.Empty;
                            var targetLanguage = translationItem.to?.ToString() ?? string.Empty;

                            TranslationDomain translation = new()
                            {
                                Text = translatedText,
                                Language = targetLanguage
                            };
                            translations.Add(translation);
                        }
                    }                    
                }

                return translations;
            }
            return [];
        }
    }

    #endregion

}