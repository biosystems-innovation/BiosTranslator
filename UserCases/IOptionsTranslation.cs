using Domain.Models;

namespace UserCases;

public interface IOptionsTranslation
{
    /// <summary>
    /// Gets the supported languages for translation
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SupportedLanguageDomain>> GetLanguages();
}