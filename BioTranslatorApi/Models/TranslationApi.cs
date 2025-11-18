using Domain.Models;

namespace BioTranslatorApi.Models;

public class TranslationApi
{
    public string Text { get; set; } = null!;
    public string Language { get; set; } = null!;

    #region Mapping
    public static TranslationApi MapFrom(TranslationDomain domain)
    {
        return new TranslationApi
        {
            Text = domain.Text,
            Language = domain.Language
        };
    }

    public static IEnumerable<TranslationApi> MapFrom(IEnumerable<TranslationDomain> domain)
    {
        var apiList = new List<TranslationApi>();
        foreach (var item in domain)
        {
            apiList.Add(MapFrom(item));
        }
        return apiList;
    }

    public static TranslationDomain MapTo(TranslationApi api)
    {
        return new TranslationDomain
        {
            Text = api.Text,
            Language = api.Language
        };
    }

    public static IEnumerable<TranslationDomain> MapTo(IEnumerable<TranslationApi> api)
    {
        var domainList = new List<TranslationDomain>();
        foreach (var item in api)
        {
            domainList.Add(MapTo(item));
        }
        return domainList;
    }

    #endregion
}