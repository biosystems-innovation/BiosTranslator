using Domain.Models;

namespace BusinessTranslator;

public interface ITextTranslationBusiness
{

    Task<IEnumerable<TranslationDomain>> Translate(string text, IEnumerable<string> to, string from, string endPoint, string key);

    Task<IEnumerable<TranslationDomain>> Translate(string text, IEnumerable<string> to, string endPoint, string key);
}