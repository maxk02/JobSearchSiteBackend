using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs.Search;
using Core.Domains.JobApplications.Values;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Core.Services.QueueService;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.AddJobApplication;

public class AddJobApplicationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICvSearchRepository cvSearchRepository,
    IPersonalFileSearchRepository personalFileSearchRepository,
    IBackgroundJobQueueService jobQueueService)
    : IRequestHandler<AddJobApplicationRequest, Result<AddJobApplicationResponse>>
{
    public async Task<Result<AddJobApplicationResponse>> Handle(AddJobApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (request.UserId != currentUserId)
            return Result<AddJobApplicationResponse>.Forbidden();

        var creationResult = JobApplication.Create(request.UserId, request.JobId, JobApplicationStatuses.Submitted);
        if (creationResult.IsFailure)
            return Result<AddJobApplicationResponse>.WithMetadataFrom(creationResult);
        var jobApplication = creationResult.Value;
        
        var requestedPersonalFilesOfUser = await context.PersonalFiles
            .Where(pf => request.PersonalFileIds.Contains(pf.Id) && pf.UserId == currentUserId)
            .ToListAsync(cancellationToken);
        
        if (!request.PersonalFileIds.All(requestedPersonalFilesOfUser.Select(x => x.Id).Contains))
        {
            return Result<AddJobApplicationResponse>.Error();
        }
        
        jobApplication.PersonalFiles = requestedPersonalFilesOfUser;
        
        await context.JobApplications.AddAsync(jobApplication, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        
        var userId = jobApplication.UserId;
        var jobId = jobApplication.JobId;
        
        try
        {
            await cvSearchRepository.AddAppliedToJobIdAsync(userId, jobId);
        }
        catch
        {
            await jobQueueService.EnqueueForIndefiniteRetriesAsync(
                () => cvSearchRepository.AddAppliedToJobIdAsync(userId, jobId),
                nameof(ICvSearchRepository));
        }

        try
        {
            await personalFileSearchRepository.AddAppliedToJobIdAsync(request.PersonalFileIds, jobId);
        }
        catch
        {
            await jobQueueService.EnqueueForIndefiniteRetriesAsync<IPersonalFileSearchRepository>(
                x => x.AddAppliedToJobIdAsync(request.PersonalFileIds, jobId));
        }

        return Result<AddJobApplicationResponse>.Success(new AddJobApplicationResponse(jobApplication.Id));
    }
}