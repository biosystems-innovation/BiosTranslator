using System.Xml;
using BusinessTranslator;
using Domain;
using Domain.Models;
using Infrastructure.Encriptors;
using Infrastructure.Extensions;

namespace UserCases;

public class TextTranslation : ITextTranslation
{
    private readonly ITextTranslationBusiness _textTranslationBusiness;
    private readonly IOptionsTranslationBusiness _optionsTranslationBusiness;
    private readonly IBioSystemsEncryptor _encryptor;

    public TextTranslation(ITextTranslationBusiness textTranslationBusiness,
        IOptionsTranslationBusiness optionsTranslationBusiness,
        IBioSystemsEncryptor encryptor)
    {
        _textTranslationBusiness = textTranslationBusiness;
        _optionsTranslationBusiness = optionsTranslationBusiness;
        _encryptor = encryptor;
    }

    public async Task<IEnumerable<TranslationDomain>> Translate(IEnumerable<string> toTranslate, string to, string from, bool sortByLanguage = false)
    {
        var validSupportedLanguages = (await ValidateSupportedLanguages(to)).ToList();
        var translations = await _textTranslationBusiness.Translate(
            toTranslate,
            validSupportedLanguages,
            from,
            AppSettingsValue.TextApi(_encryptor),
            AppSettingsValue.KeyApi(_encryptor));

        return ApplyOptionalSorting(sortByLanguage, translations, validSupportedLanguages);
    }

    public async Task<IEnumerable<TranslationDomain>> Translate(IEnumerable<string> toTranslate, string to, bool sortByLanguage = false)
    {
        var validSupportedLanguages = (await ValidateSupportedLanguages(to)).ToList();
        var translations = await _textTranslationBusiness.Translate(
            toTranslate,
            validSupportedLanguages,
            AppSettingsValue.TextApi(_encryptor),
            AppSettingsValue.KeyApi(_encryptor));

        return ApplyOptionalSorting(sortByLanguage, translations, validSupportedLanguages);
    }

    public async Task<IEnumerable<string>> ValidateSupportedLanguages(string to)
    {
        if (string.IsNullOrEmpty(to)) return [];

        var supportedLanguages = await _optionsTranslationBusiness.GetSupportedLanguages(
            AppSettingsValue.TextApi(_encryptor),
            AppSettingsValue.KeyApi(_encryptor));
        if (supportedLanguages.IsNull()) return [];
        var supportedLanguagesList = supportedLanguages!.ToList();

        var requestedLanguages = to.Split(',');
        List<string> validLanguages = new();

        foreach (var lang in requestedLanguages)
        {
            if (supportedLanguagesList.Any(x => x.Code.Equals(lang, StringComparison.OrdinalIgnoreCase)))
            {
                validLanguages.AddOnce(lang);
            }
        }

        return validLanguages;
    }

    #region Private
    private static IEnumerable<TranslationDomain> ApplyOptionalSorting(bool sortByLanguage, IEnumerable<TranslationDomain> translations,
        List<string> validSupportedLanguages)
    {
        if (sortByLanguage)
        {
            translations = translations
                .OrderBy(x => validSupportedLanguages.IndexOf(x.Language)).ToList();
        }
        return translations;
    }

    #endregion

}