using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobApplications.Dtos;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.PersonalFiles.Dtos;
using Core.Domains.PersonalFiles.Search;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Persistence;

namespace Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;

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