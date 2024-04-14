using FluentValidation;

namespace Application.Features.JobFeatures.CreateJob.NestedDTOs.AddressDTO;

public class CreateJobAddressDtoValidator : AbstractValidator<CreateJobAddressDto?>
{
    public CreateJobAddressDtoValidator()
    {
        RuleFor(x => x!.LocationId).GreaterThanOrEqualTo(1)
            .When(x => x != null && x.LocationId != null);
        
        RuleFor(x => x!.NonStandardLocationName).NotEmpty().MaximumLength(150)
            .When(x => x != null && x.LocationId is null);
        
        RuleFor(x => x!.NonStandardLocationName).Empty()
            .When(x => x != null && x.LocationId != null);
        
        RuleFor(x => x!.Line1).NotEmpty().MaximumLength(150)
            .When(x => x != null);
        
        RuleFor(x => x!.Line2).MaximumLength(150)
            .When(x => x != null && x.Line2 != null);
        
        RuleFor(x => x!.ZipCode).MaximumLength(150)
            .When(x => x != null && x.ZipCode != null);
    }
}