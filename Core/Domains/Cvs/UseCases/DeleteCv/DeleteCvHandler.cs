using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Shared.Result;

namespace Core.Domains.Cvs.UseCases.DeleteCv;

public class DeleteCvHandler(
    ICurrentAccountService currentAccountService,
    ICvSearchRepository cvSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<DeleteCvRequest, Result>
{
    public async Task<Result> Handle(DeleteCvRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var cv = await context.Cvs.FindAsync([request.CvId], cancellationToken);

        if (cv is null)
            return Result.NotFound();

        if (cv.UserId != currentUserId)
            return Result.Forbidden();

        context.Cvs.Remove(cv);
        await context.SaveChangesAsync(cancellationToken);
        
        backgroundJobService.Enqueue(
            () => cvSearchRepository.DeleteAsync(request.CvId, CancellationToken.None),
            BackgroundJobQueues.CvSearch);
        
        return Result.Success();
    }
}