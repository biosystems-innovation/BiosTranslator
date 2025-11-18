using BioTranslatorApi.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using UserCases;

namespace BioTranslatorApi.Controllers
{
    /// <summary>
    /// Controller used to manage text translation using external API
    /// External API: Microsoft Translator Text API
    /// API documentation: https://learn.microsoft.com/en-us/azure/ai-services/translator/text-translation/reference/rest-api-guide
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TranslatorController : ControllerBase
    {
        private readonly ILogger<TranslatorController> _logger;
        private readonly ITextTranslation _textTranslation;
        private readonly IOptionsTranslation _optionsTranslation;

        public TranslatorController(ILogger<TranslatorController> logger,
            ITextTranslation textTranslation,
            IOptionsTranslation optionsTranslation)
        {
            _logger = logger;
            _textTranslation = textTranslation;
            _optionsTranslation = optionsTranslation;
        }

        [HttpGet("languages")]
        public async Task<IEnumerable<SupportedLanguagesApi>> GetLanguages()
        {
            var supportedLanguages = await _optionsTranslation.GetLanguages();
            return SupportedLanguagesApi.MapFrom(supportedLanguages);
        }

        /// <summary>
        /// Translates the given text
        /// From and To languages are defined in appSettings.json
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost("{text}")]
        public async Task<IEnumerable<TranslationApi>> Translate(string text)
        {
            string to = AppSettingsValue.To;
            string from = AppSettingsValue.From;
            var translations = await _textTranslation.Translate(text, to, from);

            return TranslationApi.MapFrom(translations);
        }

        /// <summary>
        /// Translates the given text to the specified language
        /// From: autodetect
        /// </summary>
        /// <param name="text"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpPost("{text}/{to}")]
        public async Task<IEnumerable<TranslationApi>> TranslateAutoDetect(string text, string to)
        {
            var translations = await _textTranslation.Translate(text, to);
            return TranslationApi.MapFrom(translations);
        }

        /// <summary>
        /// Translates the given text from the specified language to the specified language
        /// </summary>
        /// <param name="text"></param>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        [HttpPost("{text}/{to}/{from}")]
        public async Task<IEnumerable<TranslationApi>> TranslateExplicit(string text, string to, string from)
        {
            var translations = await _textTranslation.Translate(text, to, from);
            return TranslationApi.MapFrom(translations);
        }
    }
}