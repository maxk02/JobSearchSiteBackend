using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Jobs.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Jobs.UseCases.DeleteJob;

public class DeleteJobHandler(
    ICurrentAccountService currentAccountService,
    IJobSearchRepository jobSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<DeleteJobRequest, Result>
{
    public async Task<Result> Handle(DeleteJobRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var job = await context.Jobs.FindAsync([request.JobId], cancellationToken);

        if (job is null)
            return Result.Error();

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(job.JobFolderId, currentUserId,
                    JobFolderClaim.CanEditJobsAndSubfolders.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result.Forbidden();

        context.Jobs.Remove(job);
        await context.SaveChangesAsync(cancellationToken);

        backgroundJobService.Enqueue(
            () => jobSearchRepository.DeleteAsync(request.JobId, CancellationToken.None),
            BackgroundJobQueues.JobSearch
        );

        return Result.Success();
    }
}