using Domain.ContractTypes;
using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Domain.Entities.ContractTypes;

public class ContractTypeValidator : AbstractValidator<ContractType>
{
    public ContractTypeValidator()
    {
        RuleFor(x => x.Name).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
    }
}