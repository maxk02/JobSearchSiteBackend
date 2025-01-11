using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplication;

public class UpdateJobApplicationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobApplicationRequest, Result>
{
    public async Task<Result> Handle(UpdateJobApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplicationWithJobFolderId = await context.JobApplications
            .Where(ja => ja.Id == request.JobApplicationId)
            .Select(ja => new { JobApplication = ja, JobFolderId = ja.Job!.JobFolderId })
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplicationWithJobFolderId is null)
            return Result.NotFound();

        var jobApplication = jobApplicationWithJobFolderId.JobApplication;
        context.JobApplications.Attach(jobApplication);

        var jobFolderId = jobApplicationWithJobFolderId.JobFolderId;

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderClosures
                .GetThisOrAncestorWhereUserHasClaim(jobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobsAndSubfolders.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();

        jobApplication.Status = request.Status;

        context.JobApplications.Update(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}