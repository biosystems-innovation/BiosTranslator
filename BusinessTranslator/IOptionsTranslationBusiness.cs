using Domain.Models;

namespace BusinessTranslator;

public interface IOptionsTranslationBusiness
{
    Task<IEnumerable<SupportedLanguageDomain>> GetSupportedLanguages(string endPoint, string key);
}