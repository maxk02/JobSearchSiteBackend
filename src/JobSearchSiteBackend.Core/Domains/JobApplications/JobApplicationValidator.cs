using FluentValidation;

namespace JobSearchSiteBackend.Core.Domains.JobApplications;

public class JobApplicationValidator : AbstractValidator<JobApplication>
{
    public JobApplicationValidator()
    {
        RuleFor(x => x.JobId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Status).IsInEnum();
    }
}