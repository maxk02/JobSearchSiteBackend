using FluentValidation;

namespace Domain.Entities.JobApplications;

public class JobApplicationValidator : AbstractValidator<JobApplication>
{
    public JobApplicationValidator()
    {
        RuleFor(x => x.JobId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
    }
}