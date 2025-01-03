using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.Search;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs.Search;
using Core.Domains.JobApplications.Values;
using Core.Domains.PersonalFiles;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Core.Services.QueueService;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.AddApplication;

public class AddApplicationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICvSearchRepository cvSearchRepository,
    IPersonalFileSearchRepository personalFileSearchRepository,
    IBackgroundJobQueueService jobQueueService)
    : IRequestHandler<AddApplicationRequest, Result<AddApplicationResponse>>
{
    public async Task<Result<AddApplicationResponse>> Handle(AddApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (request.UserId != currentUserId)
            return Result<AddApplicationResponse>.Forbidden();

        var creationResult = JobApplication.Create(request.UserId, request.JobId, JobApplicationStatuses.Submitted);
        if (creationResult.IsFailure)
            return Result<AddApplicationResponse>.WithMetadataFrom(creationResult);
        var jobApplication = creationResult.Value;
        
        var requestedPersonalFilesOfUser = await context.PersonalFiles
            .Where(pf => request.PersonalFileIds.Contains(pf.Id) && pf.UserId == currentUserId)
            .ToListAsync(cancellationToken);
        
        if (!request.PersonalFileIds.All(requestedPersonalFilesOfUser.Select(x => x.Id).Contains))
        {
            return Result<AddApplicationResponse>.Error();
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

        return Result<AddApplicationResponse>.Success(new AddApplicationResponse(jobApplication.Id));
    }
}