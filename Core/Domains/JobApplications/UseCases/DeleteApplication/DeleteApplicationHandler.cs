using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs.Search;
using Core.Services.Auth.Authentication;
using Core.Services.QueueService;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.DeleteApplication;

public class DeleteApplicationHandler(
    ICurrentAccountService currentAccountService,
    IJobApplicationRepository jobApplicationRepository,
    ICvSearchRepository cvSearchRepository,
    IBackgroundJobQueueService jobQueueService) : IRequestHandler<DeleteApplicationRequest, Result>
{
    public async Task<Result> Handle(DeleteApplicationRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await jobApplicationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();

        if (jobApplication.UserId != currentUserId)
            return Result.Forbidden();

        await jobApplicationRepository.RemoveAsync(jobApplication, cancellationToken);

        var userId = jobApplication.UserId;
        var jobId = jobApplication.JobId;
        
        try
        {
            await cvSearchRepository.RemoveAppliedToJobIdAsync(jobApplication.UserId, jobApplication.JobId);
        }
        catch
        {
            await jobQueueService.EnqueueForIndefiniteRetriesAsync<ICvSearchRepository>(
                x => x.RemoveAppliedToJobIdAsync(userId, jobId));
        }

        return Result.Success();
    }
}