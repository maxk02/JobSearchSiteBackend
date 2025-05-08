using FluentValidation;
using JobSearchSiteBackend.Shared.FluentValidationAddons;
using JobSearchSiteBackend.Shared.FluentValidationAddons.StringFiltering;

namespace Core.Domains.JobContractTypes;

public class JobContractTypeValidator : AbstractValidator<JobContractType>
{
    public JobContractTypeValidator()
    {
        RuleFor(x => x.NamePl).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
    }
}