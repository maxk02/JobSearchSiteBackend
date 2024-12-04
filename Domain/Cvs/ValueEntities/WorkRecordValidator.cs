using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Domain.Cvs.ValueEntities;

public class WorkRecordValidator : AbstractValidator<WorkRecord>
{
    public WorkRecordValidator()
    {
        RuleFor(x => x.Position).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols());
        
        RuleFor(x => x.Company).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Punctuation().Symbols());
        
        RuleFor(x => x.Location).Length(1, 100)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols());
        
        RuleFor(x => x.Description).Length(1, 500)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols().Newlines());

        RuleFor(x => x.Responsibilities)
            .ForEach(x => x.Length(1, 50)
                .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Punctuation().Symbols()));
    }
}