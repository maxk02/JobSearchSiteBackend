using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;

public class GetApplicationsForJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICompanyLastVisitedJobsCacheRepository cacheRepo,
    IMapper mapper) : IRequestHandler<GetApplicationsForJobQuery, Result<GetApplicationsForJobResult>>
{
    public async Task<Result<GetApplicationsForJobResult>> Handle(GetApplicationsForJobQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var jobWithApplications = await context.Jobs
            .Include(j => j.JobFolder)
            .Include(j => j.JobApplications)
            .AsNoTracking()
            .Where(j => j.Id == query.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobWithApplications is null)
            return Result.NotFound();
        
        var canEdit = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(jobWithApplications.JobFolderId, currentUserId,
                JobFolderClaim.CanManageApplications.Id)
            .AnyAsync(cancellationToken);

        if (!canEdit)
            return Result.Forbidden();
        
        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            jobWithApplications.JobFolder!.CompanyId.ToString(), jobWithApplications.Id.ToString());

        if (jobWithApplications.JobApplications!.Count == 0)
            return Result<GetApplicationsForJobResult>.NoContent();
        
        var jobApplicationDtos = mapper.Map<List<JobApplicationForManagersDto>>(jobWithApplications.JobApplications);
        
        return new GetApplicationsForJobResult(jobApplicationDtos);
    }
}