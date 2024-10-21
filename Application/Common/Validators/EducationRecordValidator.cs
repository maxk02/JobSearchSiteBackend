using Domain.Entities.Users.ValueEntities;
using FluentValidation;

namespace Application.Common.Validators;

public class EducationRecordValidator : AbstractValidator<EducationRecord?>
{
    public EducationRecordValidator()
    {
        RuleFor(x => x!.Institution).NotEmpty().MaximumLength(100).When(x => x != null);
        RuleFor(x => x!.Location).NotEmpty().MaximumLength(100).When(x => x != null);
        RuleFor(x => x!.Faculty).NotEmpty().MaximumLength(100).When(x => x != null && x.Faculty != null);
        RuleFor(x => x!.Speciality).NotEmpty().MaximumLength(100).When(x => x != null);
        RuleFor(x => x!.Degree).NotEmpty().MaximumLength(100).When(x => x != null && x.Degree != null);
        RuleFor(x => x!.Description).NotEmpty().MaximumLength(100).When(x => x != null && x.Description != null);
        
        // RuleFor(x => x!.DateStarted)
        //     .InclusiveBetween(new DateOnly(1920, 01, 01), new DateOnly(2120, 01, 01))
        //     .When(x => x != null && x.DateStarted != null);
        //
        // RuleFor(x => x!.DateFinished).
        //     InclusiveBetween(new DateOnly(1920, 01, 01), new DateOnly(2120, 01, 01))
        //     .When(x => x != null && x.DateFinished != null);
    }
}