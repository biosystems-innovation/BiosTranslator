using Domain.Models;

namespace UserCases;

public interface ITextTranslation
{
    /// <summary>
    /// Translates the given text from the specified language to the specified language
    /// </summary>
    /// <param name="text"></param>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    Task<IEnumerable<TranslationDomain>> Translate(string text, string to, string from);

    /// <summary>
    /// Translates the given text to the specified language
    /// From: autodetect
    /// </summary>
    /// <param name="text"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    Task<IEnumerable<TranslationDomain>> Translate(string text, string to);
}