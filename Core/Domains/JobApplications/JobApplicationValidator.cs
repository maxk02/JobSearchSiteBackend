using Core.Domains.JobApplications.Values;
using FluentValidation;
using Shared.FluentValidationAddons;

namespace Core.Domains.JobApplications;

public class JobApplicationValidator : AbstractValidator<JobApplication>
{
    public JobApplicationValidator()
    {
        RuleFor(x => x.JobId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Status).MustExistIn(JobApplicationStatuses.Values);
    }
}