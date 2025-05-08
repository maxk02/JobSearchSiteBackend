namespace Shared.FluentValidationAddons.Languages;

public static class CustomEnglishLanguage
{
    public const string Culture = "en";
    public const string AmericanCulture = "en-US";
    public const string BritishCulture = "en-GB";

    public static string GetTranslation(string key) => key switch
    {
        "EmailValidator" => "This property must be a valid email address.",
        "GreaterThanOrEqualValidator" => "This property must be greater than or equal to '{ComparisonValue}'.",
        "GreaterThanValidator" => "This property must be greater than '{ComparisonValue}'.",
        "LengthValidator" => "This property must be between {MinLength} and {MaxLength} characters. You entered {TotalLength} characters.",
        "MinimumLengthValidator" => "The length of this property must be at least {MinLength} characters. You entered {TotalLength} characters.",
        "MaximumLengthValidator" => "The length of this property must be {MaxLength} characters or fewer. You entered {TotalLength} characters.",
        "LessThanOrEqualValidator" => "This property must be less than or equal to '{ComparisonValue}'.",
        "LessThanValidator" => "This property must be less than '{ComparisonValue}'.",
        "NotEmptyValidator" => "This property must not be empty.",
        "NotEqualValidator" => "This property must not be equal to '{ComparisonValue}'.",
        "NotNullValidator" => "This property must not be empty.",
        "PredicateValidator" => "The specified condition was not met for this property.",
        "AsyncPredicateValidator" => "The specified condition was not met for this property.",
        "RegularExpressionValidator" => "This property is not in the correct format.",
        "EqualValidator" => "This property must be equal to '{ComparisonValue}'.",
        "ExactLengthValidator" => "This property must be {MaxLength} characters in length. You entered {TotalLength} characters.",
        "InclusiveBetweenValidator" => "This property must be between {From} and {To}. You entered {PropertyValue}.",
        "ExclusiveBetweenValidator" => "This property must be between {From} and {To} (exclusive). You entered {PropertyValue}.",
        "CreditCardValidator" => "This property is not a valid credit card number.",
        "ScalePrecisionValidator" => "This property must not be more than {ExpectedPrecision} digits in total, with allowance for {ExpectedScale} decimals. {Digits} digits and {ActualScale} decimals were found.",
        "EmptyValidator" => "This property must be empty.",
        "NullValidator" => "This property must be empty (null).",
        "EnumValidator" => "This property has a range of values which does not include '{PropertyValue}'.",
        // // Additional fallback messages used by clientside validation integration.
        // "Length_Simple" => "This property must be between {MinLength} and {MaxLength} characters.",
        // "MinimumLength_Simple" => "The length of this property must be at least {MinLength} characters.",
        // "MaximumLength_Simple" => "The length of this property must be {MaxLength} characters or fewer.",
        // "ExactLength_Simple" => "This property must be {MaxLength} characters in length.",
        // "InclusiveBetween_Simple" => "This property must be between {From} and {To}.",
        
        // Own custom validators
        "MustExistInValidator" => "This property must have one of the values that are specified in this list of allowed values: {AllowedElementsList}.",
        "MustAllExistInValidator" => "All elements of this list must have values that are present in this list of allowed values: {AllowedElementsList}.",
        "WhitelistPolicyValidator" => "Non-allowed characters were detected in this string: {NonWhitelistedCharsFound}." +
                                      " Allowed character types (unless explicitly forbidden) are: {AllowedChars}.",
        "BlacklistPolicyValidator" => "Non-allowed characters were detected in this string: {BlacklistedCharsFound}." +
                                      " Forbidden characters are: {ForbiddenChars}.",
        
        _ => "General validation error.",
    };
}