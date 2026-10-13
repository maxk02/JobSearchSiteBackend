using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;

public class GetJobDataForCurrentAccountHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetJobDataForCurrentAccountQuery, Result<GetJobDataForCurrentAccountResult>>
{
    public async Task<Result<GetJobDataForCurrentAccountResult>> Handle(GetJobDataForCurrentAccountQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var jobApplication = await context.JobApplications
            .Include(ja => ja.Location)
            .Include(ja => ja.PersonalFiles)
            .Where(ja => ja.JobId == query.JobId && ja.UserId == currentUserId)
            .SingleOrDefaultAsync(cancellationToken);

        var isBookmarked = await context.UserJobBookmarks
            .AnyAsync(ujb => ujb.JobId == query.JobId && ujb.UserId == currentUserId, cancellationToken);
    
        JobApplicationOnJobPageDto? jobApplicationDto = null;

        if (jobApplication is not null)
        {
            jobApplicationDto = new JobApplicationOnJobPageDto(
                jobApplication.Id,
                jobApplication.Location!.ToLocationDto(),
                jobApplication.DateTimeCreatedUtc,
                jobApplication.PersonalFiles!.Select(pf => pf.Id).ToList(),
                (int)jobApplication.Status
            );
        }
        
        return new GetJobDataForCurrentAccountResult(jobApplicationDto, isBookmarked);
    }
}