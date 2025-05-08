using FluentValidation;
using JobSearchSiteBackend.Shared.FluentValidationAddons.StringFiltering;

namespace JobSearchSiteBackend.Shared.FluentValidationAddons;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TElement> MustExistIn<T, TElement>
        (this IRuleBuilder<T, TElement> ruleBuilder, ICollection<TElement> collection)
    {
        return ruleBuilder.Must((_, checkedValue, context) =>
        {
            context.MessageFormatter
                .AppendArgument("AllowedElementsList", string.Join(", ", collection));

            return collection.Contains(checkedValue);
        }).WithErrorCode("MustExistInValidator");
    }

    public static IRuleBuilderOptions<T, ICollection<TElement>> MustAllExistIn<T, TElement>(
        this IRuleBuilder<T, ICollection<TElement>> ruleBuilder,
        ICollection<TElement> predefinedCollection)
    {
        return ruleBuilder.Must((_, checkedCollection, context) =>
        {
            context.MessageFormatter
                .AppendArgument("AllowedElementsList", string.Join(", ", predefinedCollection));

            var elementsNotInPredefinedCollection =
                checkedCollection.Select(x => !predefinedCollection.Contains(x)).ToList();

            bool areNonAllowedElementsFound = elementsNotInPredefinedCollection.Count > 0;
            if (areNonAllowedElementsFound)
            {
                context.MessageFormatter
                    .AppendArgument("NonAllowedElementsFound", string.Join(", ", elementsNotInPredefinedCollection));
            }

            return areNonAllowedElementsFound;
        }).WithErrorCode("MustAllExistInValidator");
    }
    
    public static IRuleBuilderOptions<T, string> WhitelistPolicy<T>
        (this IRuleBuilder<T, string> ruleBuilder, WhitelistPolicy whitelistPolicy)
    {
        return ruleBuilder.Must((_, checkedValue, context) =>
        {
            whitelistPolicy.Check(checkedValue);

            if (whitelistPolicy.NonWhitelistedCharsFound.Count > 0)
            {
                context.MessageFormatter
                    .AppendArgument("NonWhitelistedCharsFound", string.Join(", ", whitelistPolicy.NonWhitelistedCharsFound));
                context.MessageFormatter
                    .AppendArgument("AllowedChars", string.Join(", ", whitelistPolicy.AllowedCharTypes));
            }
            
            return whitelistPolicy.NonWhitelistedCharsFound.Count == 0;
        }).WithErrorCode("WhitelistPolicyValidator");
    }
    
    public static IRuleBuilderOptions<T, string> BlacklistPolicy<T>
        (this IRuleBuilder<T, string> ruleBuilder, BlacklistPolicy blacklistPolicy)
    {
        return ruleBuilder.Must((_, checkedValue, context) =>
        {
            blacklistPolicy.Check(checkedValue);

            if (blacklistPolicy.BlacklistedCharsFound.Count > 0)
            {
                context.MessageFormatter
                    .AppendArgument("BlacklistedCharsFound", string.Join(", ", blacklistPolicy.BlacklistedCharsFound));
                context.MessageFormatter
                    .AppendArgument("ForbiddenChars", string.Join(", ", blacklistPolicy.ForbiddenCharTypes));
                
            }
            
            return blacklistPolicy.BlacklistedCharsFound.Count == 0;
        }).WithErrorCode("BlacklistPolicyValidator");
    }
}