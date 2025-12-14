using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;

public class AddJobApplicationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<AddJobApplicationCommand, Result<AddJobApplicationResult>>
{
    public async Task<Result<AddJobApplicationResult>> Handle(AddJobApplicationCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = new JobApplication(currentUserId, command.JobId, JobApplicationStatus.Submitted);

        var requestedPersonalFilesOfUser = await context.PersonalFiles
            .Where(pf => command.PersonalFileIds.Contains(pf.Id) && pf.UserId == currentUserId)
            .ToListAsync(cancellationToken);

        if (!command.PersonalFileIds.All(requestedPersonalFilesOfUser.Select(x => x.Id).Contains))
        {
            return Result.Error();
        }

        jobApplication.PersonalFiles = requestedPersonalFilesOfUser;

        await context.JobApplications.AddAsync(jobApplication, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result<AddJobApplicationResult>.Success(new AddJobApplicationResult(jobApplication.Id));
    }
}