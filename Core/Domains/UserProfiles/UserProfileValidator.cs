using FluentValidation;
using Shared.FluentValidationAddons;
using Shared.FluentValidationAddons.StringFiltering;

namespace Core.Domains.UserProfiles;

public class UserProfileValidator : AbstractValidator<UserProfile>
{
    public UserProfileValidator()
    {
        RuleFor(x => x.Id).GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.FirstName).Length(1, 40)
            .WhitelistPolicy(new WhitelistPolicy().Letters());
        
        // RuleFor(x => x.MiddleName).Length(1, 40)
        //     .WhitelistPolicy(new WhitelistPolicy().Letters());
        
        RuleFor(x => x.LastName).Length(1, 40)
            .WhitelistPolicy(new WhitelistPolicy().Letters());

        RuleFor(x => x.Email).Length(1, 50).EmailAddress();

        // When(x => x.Phone != null, () =>
        // {
        //     RuleFor(x => x.Phone!).SetValidator(new PhoneValidator());
        // });
    }
}