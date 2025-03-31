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

        // When(x => x.SalaryRecord != null, () =>
        // {
        //     RuleFor(x => x.SalaryRecord!).SetValidator(new SalaryRecordValidator());
        // });

        RuleFor(x => x.Responsibilities)
            .ForEach(x => x.Length(1, 50)
                .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces()));
        
        RuleFor(x => x.Requirements)
            .ForEach(x => x.Length(1, 50)
                .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces()));
        
        RuleFor(x => x.NiceToHaves)
            .ForEach(x => x.Length(1, 50)
                .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces()));
    }
}