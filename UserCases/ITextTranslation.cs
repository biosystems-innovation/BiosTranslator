using Domain.Models;

namespace UserCases;

public interface ITextTranslation
{
    /// <summary>
    /// Translates the given collection of texts from the specified language to the specified language
    ///
    /// Optionally output can be sorted by language
    /// </summary>
    /// <param name="toTranslate"></param>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <param name="sortByLanguage"></param>
    /// <returns></returns>
    Task<IEnumerable<TranslationDomain>> Translate(IEnumerable<string> toTranslate, string to, string from, bool sortByLanguage = false);

    /// <summary>
    /// Translates the given collection of texts to the specified language
    ///
    /// Optionally output can be sorted by language
    /// From: autodetect
    /// </summary>
    /// <param name="toTranslate"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    Task<IEnumerable<TranslationDomain>> Translate(IEnumerable<string> toTranslate, string to, bool sortByLanguage = false);

    /// <summary>
    /// Validates the requested languages against the supported languages
    /// Also removes duplicates
    ///
    /// Example:
    /// Entry is a string with comma separated language codes: "en,es,fr,"fake,"en"
    /// Returns a collection of valid language codes: item[0] = "en",item[1] = "es",item[2] = "fr"
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    Task<IEnumerable<string>> ValidateSupportedLanguages(string to);
}