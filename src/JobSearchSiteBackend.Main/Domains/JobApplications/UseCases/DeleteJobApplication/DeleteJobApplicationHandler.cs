using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.DeleteJobApplication;

public class DeleteJobApplicationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<DeleteJobApplicationCommand, Result>
{
    public async Task<Result> Handle(DeleteJobApplicationCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await context.JobApplications
            .Where(ja => ja.Id == command.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();

        if (jobApplication.UserId != currentUserId)
            return Result.Forbidden();
        
        jobApplication.IsDeleted = true;
        
        context.JobApplications.Update(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}