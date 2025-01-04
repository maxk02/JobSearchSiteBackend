using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Core.Services.QueueService;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

public class UpdateJobApplicationFilesHandler(
    ICurrentAccountService currentAccountService,
    IPersonalFileSearchRepository personalFileSearchRepository,
    IBackgroundJobQueueService jobQueueService,
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
        
        var oldPersonalFileIds = jobApplication.PersonalFiles?.Select(x => x.Id).ToList() ?? [];
        jobApplication.PersonalFiles = newPersonalFiles;
        context.Update(jobApplication);
        await context.SaveChangesAsync(cancellationToken);
        
        
        var jobId = jobApplication.JobId;

        try
        {
            await personalFileSearchRepository.RemoveAppliedToJobIdAsync(oldPersonalFileIds, jobId);
        }
        catch
        {
            await jobQueueService.EnqueueForIndefiniteRetriesAsync<IPersonalFileSearchRepository>(
                x => x.RemoveAppliedToJobIdAsync(oldPersonalFileIds, jobId));
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
        

        return Result.Success();
    }
}