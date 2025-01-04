using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs.Search;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Core.Services.QueueService;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.DeleteJobApplication;

public class DeleteJobApplicationHandler(
    ICurrentAccountService currentAccountService,
    ICvSearchRepository cvSearchRepository,
    IPersonalFileSearchRepository personalFileSearchRepository,
    MainDataContext context,
    IBackgroundJobQueueService jobQueueService) : IRequestHandler<DeleteJobApplicationRequest, Result>
{
    public async Task<Result> Handle(DeleteJobApplicationRequest request, CancellationToken cancellationToken = default)
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

        var oldPersonalFileIds = jobApplication.PersonalFiles?.Select(x => x.Id).ToList() ?? [];
        context.JobApplications.Remove(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

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
        
        try
        {
            await personalFileSearchRepository.RemoveAppliedToJobIdAsync(oldPersonalFileIds, jobId);
        }
        catch
        {
            await jobQueueService.EnqueueForIndefiniteRetriesAsync<IPersonalFileSearchRepository>(
                x => x.RemoveAppliedToJobIdAsync(oldPersonalFileIds, jobId));
        }

        return Result.Success();
    }
}