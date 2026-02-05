using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.RemoveJobApplicationTag;

public class RemoveJobApplicationTagHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<RemoveJobApplicationTagCommand, Result>
{
    public async Task<Result> Handle(RemoveJobApplicationTagCommand command,
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
        
        var removedTag = jobApplicationWithTags.Tags!.SingleOrDefault(t => t.Tag == command.Tag);
        
        if (removedTag is null)
            return Result.Error();
        
        jobApplicationWithTags.Tags!.Remove(removedTag);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}