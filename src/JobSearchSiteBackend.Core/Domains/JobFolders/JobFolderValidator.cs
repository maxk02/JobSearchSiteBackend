using FluentValidation;
using JobSearchSiteBackend.Shared.FluentValidationAddons;
using JobSearchSiteBackend.Shared.FluentValidationAddons.StringFiltering;

namespace JobSearchSiteBackend.Core.Domains.JobFolders;

public class JobFolderValidator : AbstractValidator<JobFolder>
{
    public JobFolderValidator()
    {
        RuleFor(x => x.CompanyId).GreaterThanOrEqualTo(1);
        // RuleFor(x => x.ParentId).GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.Name).Length(1, 50)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Digits().Spaces().Symbols().Punctuation());
        
        RuleFor(x => x.Description).Length(1, 300)
            .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Punctuation().Symbols().Newlines());
    }
}