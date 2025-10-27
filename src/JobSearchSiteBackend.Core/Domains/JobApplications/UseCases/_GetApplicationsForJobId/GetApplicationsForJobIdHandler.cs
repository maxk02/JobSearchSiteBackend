using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases._GetApplicationsForJobId;

public class GetApplicationsForJobIdHandler(
    ICurrentAccountService currentAccountService,
    IPersonalFileSearchRepository personalFileSearchRepository,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetApplicationsForJobIdRequest, Result<GetApplicationsForJobIdResponse>>
{
    public async Task<Result<GetApplicationsForJobIdResponse>> Handle(GetApplicationsForJobIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolderId = await context.Jobs
            .AsNoTracking()
            .Where(job => job.Id == request.JobId)
            .Select(j => j.JobFolderId)
            .SingleOrDefaultAsync(cancellationToken);

        if (jobFolderId == 0)
            return Result<GetApplicationsForJobIdResponse>.NotFound();

        var hasPermissionInCurrentFolderOrAncestors =
            await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(jobFolderId, currentUserId,
                    JobFolderClaim.CanManageApplications.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInCurrentFolderOrAncestors)
            return Result<GetApplicationsForJobIdResponse>.Forbidden();

        var query = context.JobApplications
            .Include(ja => ja.PersonalFiles)
            .Where(ja => ja.JobId == request.JobId);
        
        if (!string.IsNullOrEmpty(request.Query))
        {
            var pfIdsFromSql = await context.JobApplications
                .Where(ja => ja.JobId == request.JobId)
                .SelectMany(ja => ja.PersonalFiles!).Select(pf => pf.Id)
                .ToListAsync(cancellationToken);
            
            var pfIdHits = await personalFileSearchRepository
                .SearchFromIdsAsync(pfIdsFromSql, request.Query, cancellationToken);
            
            query = query.Where(ja => ja.PersonalFiles!.Any(pf => pfIdHits.Contains(pf.Id)));
        }

        var count = await query.CountAsync(cancellationToken);

        var jobApplicationDtos = await query
            .OrderByDescending(ja => ja.DateTimeCreatedUtc)
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .AsNoTracking()
            .ProjectTo<JobApplicationForManagersDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var paginationResponse = new PaginationResponse(request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize, count);

        var response = new GetApplicationsForJobIdResponse(jobApplicationDtos, paginationResponse);

        return response;
    }
}