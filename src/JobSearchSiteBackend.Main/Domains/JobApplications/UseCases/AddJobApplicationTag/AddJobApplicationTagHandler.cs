using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.CompanyClaims;
using JobSearchSiteBackend.Main.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.AddJobApplicationTag;

public class AddJobApplicationTagHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<AddJobApplicationTagCommand, Result>
{
    public async Task<Result> Handle(AddJobApplicationTagCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var jobApplicationWithTags = await context.JobApplications
            .Include(ja => ja.Tags)
            .Include(ja => ja.Job)
            .Where(ja => ja.Id == command.JobApplicationId)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (jobApplicationWithTags is null)
            return Result.NotFound();
        
        var hasPermissionInRequestedCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == jobApplicationWithTags.Job!.CompanyId
                              && ucc.UserId == currentUserId
                              && ucc.ClaimId == CompanyClaim.CanManageApplications.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();
        
        if (jobApplicationWithTags.Tags!.Select(t => t.Tag).Contains(command.Tag))
            return Result.Success();
        
        var jobApplicationTag = new JobApplicationTag(command.JobApplicationId, command.Tag);
        
        jobApplicationWithTags.Tags!.Add(jobApplicationTag);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}