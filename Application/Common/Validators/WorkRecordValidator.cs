using Domain.JSONEntities;
using FluentValidation;

namespace Application.Common.Validators;

public class WorkRecordValidator : AbstractValidator<WorkRecord?>
{
    public WorkRecordValidator()
    {
        RuleFor(x => x!.Company).NotEmpty().MaximumLength(100).When(x => x != null);
        RuleFor(x => x!.Location).NotEmpty().MaximumLength(100).When(x => x != null);
        RuleFor(x => x!.Responsibilities)
            .ForEach(x => x.NotEmpty().MaximumLength(100))
            .When(x => x != null && x.Responsibilities != null);
        RuleFor(x => x!.Description).NotEmpty().MaximumLength(500)
            .When(x => x != null && x.Description != null);
    }
}