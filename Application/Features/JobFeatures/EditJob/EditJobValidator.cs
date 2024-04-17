using Application.Common.Validators;
using Application.Features.JobFeatures.EditJob.NestedDTOs.AddressDTO;
using FluentValidation;

namespace Application.Features.JobFeatures.EditJob;

public sealed class EditJobValidator : AbstractValidator<EditJobRequest>
{
    public EditJobValidator()
    {
        RuleFor(x => x.Id).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x => x.CategoryId).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DateTimeExpiringUtc).NotNull()
            .InclusiveBetween(DateTime.UtcNow.AddHours(3), DateTime.UtcNow.AddDays(60));

        RuleFor(x => x.SalaryInfo!)
            .SetValidator(new SalaryRecordValidator())
            .When(x => x.SalaryInfo != null);
        
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        
        RuleFor(x => x.Responsibilities!)
            .Must(x => x.Count <= 20)
            .ForEach(x => x.NotEmpty().MaximumLength(50))
            .When(x => x.Responsibilities != null);
        
        RuleFor(x => x.Requirements!)
            .Must(x => x.Count <= 20)
            .ForEach(x => x.NotEmpty().MaximumLength(50))
            .When(x => x.Requirements != null);
        
        RuleFor(x => x.Advantages!)
            .Must(x => x.Count <= 20)
            .ForEach(x => x.NotEmpty().MaximumLength(50))
            .When(x => x.Advantages != null);

        RuleFor(x => x.IsHidden).NotNull();
        
        RuleFor(x => x.Addresses!)
            .ForEach(x => x.NotNull().SetValidator(new EditJobAddressDtoValidator()))
            .When(x => x.Addresses != null);

        RuleFor(x => x.ContractTypeIds!)
            .ForEach(x => x.NotNull().GreaterThanOrEqualTo(1))
            .When(x => x.ContractTypeIds != null);
    }
}