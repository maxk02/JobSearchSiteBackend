using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

public class UpdateJobApplicationFilesHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobApplicationFilesRequest, Result>
{
    public async Task<Result> Handle(UpdateJobApplicationFilesRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await context.JobApplications
            .Include(ja => ja.PersonalFiles)
            .Where(ja => ja.Id == request.Id)
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
            return Result.Error();
        }
        
        jobApplication.PersonalFiles = newPersonalFiles;
        context.Update(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}