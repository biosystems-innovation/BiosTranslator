using Domain;

namespace BioTranslatorApi.Configuration;

public class ApplicationConfigurator
{
    public static void SetupConfigurationRoot()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        AppSettingsValue.Configuration = configurationBuilder.Build();
    }

    public static string GetApiUrl()
    {
        return AppSettingsValue.UrlApi();
    }
    
    public static int GetApiPort()
    {
        return AppSettingsValue.PortApi();
    }
}