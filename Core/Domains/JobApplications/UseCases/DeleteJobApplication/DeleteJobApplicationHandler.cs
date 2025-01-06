using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs.Search;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.DeleteJobApplication;

public class DeleteJobApplicationHandler(
    IJwtCurrentAccountService jwtCurrentAccountService,
    ICvSearchRepository cvSearchRepository,
    IPersonalFileSearchRepository personalFileSearchRepository,
    MainDataContext context,
    IBackgroundJobService backgroundJobService) : IRequestHandler<DeleteJobApplicationRequest, Result>
{
    public async Task<Result> Handle(DeleteJobApplicationRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = jwtCurrentAccountService.GetIdOrThrow();

        var jobApplication = await context.JobApplications
            .Include(ja => ja.PersonalFiles)
            .Where(ja => ja.Id == request.JobApplicationId)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();

        if (jobApplication.UserId != currentUserId)
            return Result.Forbidden();

        var userId = jobApplication.UserId;
        var jobId = jobApplication.JobId;

        var oldPersonalFileIds = jobApplication.PersonalFiles?.Select(x => x.Id).ToList() ?? [];
        context.JobApplications.Remove(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        backgroundJobService.Enqueue(
            () => cvSearchRepository.RemoveAppliedToJobIdAsync(userId, jobId, CancellationToken.None),
            BackgroundJobQueues.CvSearch);

        backgroundJobService.Enqueue(
            () => personalFileSearchRepository.RemoveAppliedToJobIdAsync(oldPersonalFileIds, jobId,
                CancellationToken.None),
            BackgroundJobQueues.PersonalFileTextExtractionAndSearch);

        return Result.Success();
    }
}