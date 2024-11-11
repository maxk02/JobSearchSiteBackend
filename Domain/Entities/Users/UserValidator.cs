using Domain.Entities.Users.ValueEntities;
using Domain.Shared.ValueEntities;
using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Domain.Entities.Users;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName).Length(1, 40)
            .WhitelistPolicy(new WhitelistPolicy().Letters());
        
        RuleFor(x => x.MiddleName).Length(1, 40)
            .WhitelistPolicy(new WhitelistPolicy().Letters());
        
        RuleFor(x => x.LastName).Length(1, 40)
            .WhitelistPolicy(new WhitelistPolicy().Letters());

        RuleFor(x => x.DateOfBirth)
            .InclusiveBetween(new DateOnly(1940, 01, 01),
                DateOnly.FromDateTime(DateTime.Today.AddYears(-15)));

        RuleFor(x => x.Email).Length(1, 50).EmailAddress();

        RuleFor(x => x.Phone).Length(1, 20)
            .WhitelistPolicy(new WhitelistPolicy().Digits().Spaces().CustomChars(['+', '-', '(', ')']));

        RuleFor(x => x.Bio).Length(1, 100)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols());

        When(x => x.SalaryRecord != null, () =>
        {
            RuleFor(x => x.SalaryRecord!).SetValidator(new SalaryRecordValidator());
        });
        
        When(x => x.EmploymentTypeRecord != null, () =>
        {
            RuleFor(x => x.EmploymentTypeRecord!).SetValidator(new EmploymentTypeRecordValidator());
        });

        RuleFor(x => x.EducationRecords)
            .ForEach(x => x.SetValidator(new EducationRecordValidator()));
        
        RuleFor(x => x.WorkRecords)
            .ForEach(x => x.SetValidator(new WorkRecordValidator()));

        RuleFor(x => x.Skills)
            .ForEach(x => x.Length(1, 50)
                .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Symbols()));
    }
}