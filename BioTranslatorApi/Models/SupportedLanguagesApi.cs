using Domain.Models;

namespace BioTranslatorApi.Models;

public class SupportedLanguagesApi
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    #region Mapping
    
    public static SupportedLanguagesApi MapFrom(SupportedLanguageDomain modelDomain)
    {
        SupportedLanguagesApi modelApi = new SupportedLanguagesApi
        {
            Code = modelDomain.Code,
            Name = modelDomain.Name
        };
        return modelApi;
    }

    public static IEnumerable<SupportedLanguagesApi> MapFrom(IEnumerable<SupportedLanguageDomain> modelDomain)
    {
        List<SupportedLanguagesApi> modelApiList = new List<SupportedLanguagesApi>();
        foreach (var model in modelDomain)
        {
            modelApiList.Add(MapFrom(model));
        }
        return modelApiList;
    }

    public static SupportedLanguageDomain MapTo(SupportedLanguagesApi modelApi)
    {
        SupportedLanguageDomain modelDomain = new SupportedLanguageDomain
        {
            Code = modelApi.Code,
            Name = modelApi.Name
        };
        return modelDomain;
    }

    public static IEnumerable<SupportedLanguageDomain> MapFrom(IEnumerable<SupportedLanguagesApi> modelApi)
    {
        List<SupportedLanguageDomain> modelDomainList = new List<SupportedLanguageDomain>();
        foreach (var model in modelApi)
        {
            modelDomainList.Add(MapTo(model));
        }
        return modelDomainList;
    }
    #endregion

}