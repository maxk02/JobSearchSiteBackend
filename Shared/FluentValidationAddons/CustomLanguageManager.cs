using System.Globalization;
using FluentValidation.Resources;
using Shared.FluentValidationAddons.Languages;

namespace Shared.FluentValidationAddons;

public class CustomLanguageManager : LanguageManager
{
    public CustomLanguageManager()
    {
        Enabled = true;
    }

    private static string GetCustomTranslation(string? culture, string key)
    {
        return culture switch
        {
            // CustomEnglishLanguage.AmericanCulture => CustomEnglishLanguage.GetTranslation(key),
            // CustomEnglishLanguage.BritishCulture => CustomEnglishLanguage.GetTranslation(key),
            // CustomEnglishLanguage.Culture => CustomEnglishLanguage.GetTranslation(key),
            _ => CustomEnglishLanguage.GetTranslation(key),
        };
    }

    public override string GetString(string key, CultureInfo? culture = null)
    {
        return GetCustomTranslation(culture?.Name, key);
    }
}