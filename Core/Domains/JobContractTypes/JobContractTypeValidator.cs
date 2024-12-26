using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Core.Domains.JobContractTypes;

public class JobContractTypeValidator : AbstractValidator<JobContractType>
{
    public JobContractTypeValidator()
    {
        RuleFor(x => x.Name).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
    }
}