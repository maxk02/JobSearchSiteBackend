using Core.Domains._Shared.ValueEntities;
using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Core.Domains.Jobs;

public class JobValidator : AbstractValidator<Job>
{
    public JobValidator()
    {
        RuleFor(x => x.CategoryId).GreaterThanOrEqualTo(1);

        RuleFor(x => x.Title).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces());
        
        RuleFor(x => x.Description).Length(30, 500)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation().Newlines());

        RuleFor(x => x.DateTimeExpiringUtc)
            .InclusiveBetween(DateTime.UtcNow, DateTime.UtcNow.AddMonths(6));

        When(x => x.SalaryRecord != null, () =>
        {
            RuleFor(x => x.SalaryRecord!).SetValidator(new SalaryRecordValidator());
        });
        
        When(x => x.EmploymentTypeRecord != null, () =>
        {
            RuleFor(x => x.EmploymentTypeRecord!).SetValidator(new EmploymentTypeRecordValidator());
        });

        RuleFor(x => x.Responsibilities)
            .ForEach(x => x.Length(1, 50)
                .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces()));
        
        RuleFor(x => x.Requirements)
            .ForEach(x => x.Length(1, 50)
                .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces()));
        
        RuleFor(x => x.Advantages)
            .ForEach(x => x.Length(1, 50)
                .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces()));
    }
}