using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;

public class GetApplicationsForJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetApplicationsForJobRequest, Result<GetApplicationsForJobResponse>>
{
    public async Task<Result<GetApplicationsForJobResponse>> Handle(GetApplicationsForJobRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetId();

        if (currentUserId is null)
            return Result.Forbidden();
        
        var jobWithApplications = await context.Jobs
            .Include(j => j.JobApplications)
            .AsNoTracking()
            .Where(j => j.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobWithApplications is null)
            return Result.NotFound();
        
        var canEdit = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(jobWithApplications.JobFolderId, currentUserId.Value,
                JobFolderClaim.CanManageApplications.Id)
            .AnyAsync(cancellationToken);

        if (!canEdit)
            return Result.Forbidden();

        if (jobWithApplications.JobApplications!.Count == 0)
            return Result<GetApplicationsForJobResponse>.NoContent();
        
        var jobApplicationDtos = mapper.Map<List<JobApplicationForManagersDto>>(jobWithApplications.JobApplications);
        
        return new GetApplicationsForJobResponse(jobApplicationDtos);
    }
}