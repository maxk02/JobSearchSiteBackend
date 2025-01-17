using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs.Search;
using Core.Domains.JobApplications.Enums;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.AddJobApplication;

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
            return Result<AddJobApplicationResponse>.Forbidden();

        var jobApplication = new JobApplication(request.UserId, request.JobId, JobApplicationStatusEnum.Submitted);

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

        return Result<AddJobApplicationResponse>.Success(new AddJobApplicationResponse(jobApplication.Id));
    }
}