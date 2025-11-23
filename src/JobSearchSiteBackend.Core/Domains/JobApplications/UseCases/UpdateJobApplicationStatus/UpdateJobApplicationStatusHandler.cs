using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;

public class UpdateJobApplicationStatusHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobApplicationStatusCommand, Result>
{
    public async Task<Result> Handle(UpdateJobApplicationStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplicationWithJobFolderId = await context.JobApplications
            .Where(ja => ja.Id == command.Id)
            .Select(ja => new { JobApplication = ja, JobFolderId = ja.Job!.JobFolderId })
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplicationWithJobFolderId is null)
            return Result.NotFound();

        var jobApplication = jobApplicationWithJobFolderId.JobApplication;
        
        var jobFolderId = jobApplicationWithJobFolderId.JobFolderId;

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(jobFolderId, currentUserId,
                    JobFolderClaim.CanManageApplications.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();
        
        context.JobApplications.Attach(jobApplication);

        jobApplication.Status = command.Status;

        context.JobApplications.Update(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}