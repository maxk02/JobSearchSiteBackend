using Domain._Shared.ValueEntities;
using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Domain.Users;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.AccountId).NotEmpty();
        
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

        When(x => x.Phone != null, () =>
        {
            RuleFor(x => x.Phone!).SetValidator(new PhoneValidator());
        });

        RuleFor(x => x.Bio).Length(1, 100)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols());
    }
}