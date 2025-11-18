using BusinessTranslator;
using Domain;
using Domain.Models;
using Infrastructure.Encriptors;

namespace UserCases;

public class OptionsTranslation : IOptionsTranslation
{
    private readonly IOptionsTranslationBusiness _optionsTranslationBusiness;
    private readonly IBioSystemsEncryptor _encryptor;

    public OptionsTranslation(IOptionsTranslationBusiness optionsTranslationBusiness, 
        IBioSystemsEncryptor encryptor)
    {
        _optionsTranslationBusiness = optionsTranslationBusiness;
        _encryptor = encryptor;
    }


    public async Task<IEnumerable<SupportedLanguageDomain>> GetLanguages()
    {
        string endPoint = AppSettingsValue.TextApi(_encryptor);
        string key = AppSettingsValue.KeyApi(_encryptor);
        return await _optionsTranslationBusiness.GetSupportedLanguages(endPoint, key);
    }
}