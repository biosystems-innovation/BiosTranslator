using Domain.Models;

namespace BusinessTranslator;

public interface ITextTranslationBusiness
{
    //Translate text collection
    Task<IEnumerable<TranslationDomain>> Translate(IEnumerable<string> toTranslate, IEnumerable<string> to, string from, string endPoint, string key);
    Task<IEnumerable<TranslationDomain>> Translate(IEnumerable<string> toTranslate, IEnumerable<string> to, string endPoint, string key);
}