using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

public class UpdateJobApplicationFilesHandler(
    ICurrentAccountService currentAccountService,
    IPersonalFileSearchRepository personalFileSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<UpdateJobApplicationFilesRequest, Result>
{
    public async Task<Result> Handle(UpdateJobApplicationFilesRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await context.JobApplications
            .Include(ja => ja.PersonalFiles)
            .Where(ja => ja.Id == request.JobApplicationId)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();

        if (jobApplication.UserId != currentUserId)
            return Result.Forbidden();

        var newPersonalFiles = await context.PersonalFiles
            .Where(pf => request.PersonalFileIds.Contains(pf.Id))
            .ToListAsync(cancellationToken);

        if (!request.PersonalFileIds.All(newPersonalFiles.Select(x => x.Id).Contains) ||
            newPersonalFiles.Any(x => x.UserId != currentUserId))
        {
            return Result<long>.Error();
        }

        var jobId = jobApplication.JobId;

        var oldPersonalFileIds = jobApplication.PersonalFiles?.Select(x => x.Id).ToList() ?? [];
        jobApplication.PersonalFiles = newPersonalFiles;
        context.Update(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        backgroundJobService.Enqueue(
            () => personalFileSearchRepository.RemoveAppliedToJobIdAsync(oldPersonalFileIds, jobId,
                CancellationToken.None),
            BackgroundJobQueues.PersonalFileTextExtractionAndSearch);

        backgroundJobService.Enqueue(
            () => personalFileSearchRepository.AddAppliedToJobIdAsync(request.PersonalFileIds, jobId,
                CancellationToken.None),
            BackgroundJobQueues.PersonalFileTextExtractionAndSearch);

        return Result.Success();
    }
}