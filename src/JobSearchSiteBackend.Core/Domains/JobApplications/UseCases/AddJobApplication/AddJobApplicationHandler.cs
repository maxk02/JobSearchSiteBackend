using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;

public class AddJobApplicationHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<AddJobApplicationRequest, Result<AddJobApplicationResponse>>
{
    public async Task<Result<AddJobApplicationResponse>> Handle(AddJobApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (request.UserId != currentUserId)
            return Result.Forbidden();

        var jobApplication = new JobApplication(request.UserId, request.JobId, JobApplicationStatus.Submitted);

        var requestedPersonalFilesOfUser = await context.PersonalFiles
            .Where(pf => request.PersonalFileIds.Contains(pf.Id) && pf.UserId == currentUserId)
            .ToListAsync(cancellationToken);

        if (!request.PersonalFileIds.All(requestedPersonalFilesOfUser.Select(x => x.Id).Contains))
        {
            return Result.Error();
        }

        jobApplication.PersonalFiles = requestedPersonalFilesOfUser;

        await context.JobApplications.AddAsync(jobApplication, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result<AddJobApplicationResponse>.Success(new AddJobApplicationResponse(jobApplication.Id));
    }
}