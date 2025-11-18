
using Infrastructure.Encriptors;
using Microsoft.Extensions.Configuration;

namespace Domain;

public class AppSettingsValue
{
    public static IConfigurationRoot Configuration { private get; set; } = null!;

    public static string From => Configuration.GetSection("Translator:From").Value ?? string.Empty;
    public static string To => Configuration.GetSection("Translator:To").Value ?? string.Empty;
    public static string AllowedHosts => Configuration.GetSection("AllowedHosts").Value ?? string.Empty;

    public static string KeyApi(IBioSystemsEncryptor encryptor)
    {
        var key = Configuration.GetSection("Translator:KeyApi").Value ?? string.Empty;
        if (string.IsNullOrEmpty(key)) return string.Empty;

        return encryptor.Decrypt(key);
    }

    public static string TextApi(IBioSystemsEncryptor encryptor)
    {
        var text = Configuration.GetSection("Translator:TextApi").Value ?? string.Empty;
        if (string.IsNullOrEmpty(text)) return string.Empty;

        return encryptor.Decrypt(text);
    }

    public static string DocumentApi(IBioSystemsEncryptor encryptor)
    {
        var document = Configuration.GetSection("Translator:DocumentApi").Value ?? string.Empty;
        if (string.IsNullOrEmpty(document)) return string.Empty;

        return encryptor.Decrypt(document);
    }

}