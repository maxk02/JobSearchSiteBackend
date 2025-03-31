using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.JobApplications.UseCases.DeleteJobApplication;

public class DeleteJobApplicationHandler(
    ICurrentAccountService currentAccountService,
    IPersonalFileSearchRepository personalFileSearchRepository,
    MainDataContext context,
    IBackgroundJobService backgroundJobService) : IRequestHandler<DeleteJobApplicationRequest, Result>
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
        
        context.JobApplications.Remove(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}