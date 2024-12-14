using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Core.Cvs.ValueEntities;

public class EducationRecordValidator : AbstractValidator<EducationRecord>
{
    public EducationRecordValidator()
    {
        RuleFor(x => x.Institution).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces());
        
        RuleFor(x => x.Location).Length(1, 100)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols());
        
        RuleFor(x => x.Faculty).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols());
        
        RuleFor(x => x.Speciality).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols());
        
        RuleFor(x => x.Degree).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols());
        
        RuleFor(x => x.Description).Length(1, 500)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols().Newlines());

        RuleFor(x => x.DateOfStart)
            .InclusiveBetween(new DateOnly(1940, 01, 01), new DateOnly(DateTime.Today.Year + 2, 01, 01))
            .LessThan(x => x.DateOfFinish);

        RuleFor(x => x.DateOfFinish)
            .InclusiveBetween(new DateOnly(1940, 01, 01), new DateOnly(DateTime.Today.Year + 5, 01, 01))
            .GreaterThan(x => x.DateOfStart);
    }
}