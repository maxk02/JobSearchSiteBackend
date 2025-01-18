using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs.Dtos;
using Core.Domains.Cvs.Search;
using Core.Domains.JobApplications.Dtos;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.PersonalFiles.Dtos;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;

public class GetApplicationsForJobIdHandler(
    ICurrentAccountService currentAccountService,
    IPersonalFileSearchRepository personalFileSearchRepository,
    ICvSearchRepository cvSearchRepository,
    MainDataContext context) : IRequestHandler<GetApplicationsForJobIdRequest, Result<GetApplicationsForJobIdResponse>>
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
            .Include(ja => ja.User!).ThenInclude(u => u.Cvs)!.ThenInclude(cv => cv.SalaryRecord)
            .Include(ja => ja.User!).ThenInclude(u => u.Cvs)!.ThenInclude(cv => cv.EmploymentTypeRecord)
            .Include(ja => ja.User!).ThenInclude(u => u.Cvs)!.ThenInclude(cv => cv.EducationRecords)
            .Include(ja => ja.User!).ThenInclude(u => u.Cvs)!.ThenInclude(cv => cv.WorkRecords)
            .Include(ja => ja.User!).ThenInclude(u => u.Cvs)!.ThenInclude(cv => cv.Skills)
            .Include(ja => ja.PersonalFiles)
            .Where(ja => ja.JobId == request.JobId);
        
        if (!string.IsNullOrEmpty(request.Query))
        {
            var pfIdsFromSql = await context.JobApplications
                .Where(ja => ja.JobId == request.JobId)
                .SelectMany(ja => ja.PersonalFiles!).Select(pf => pf.Id)
                .ToListAsync(cancellationToken);
            
            var cvIdsFromSql = await context.JobApplications
                .Where(ja => ja.JobId == request.JobId)
                .SelectMany(ja => ja.User!.Cvs!).Where(cv => cv.IsPublic).Select(cv => cv.Id)
                .ToListAsync(cancellationToken);
            
            var pfIdHits = await personalFileSearchRepository
                .SearchFromIdsAsync(pfIdsFromSql, request.Query, cancellationToken);
            
            var cvIdHits = await cvSearchRepository
                .SearchFromIdsAsync(cvIdsFromSql, request.Query, cancellationToken);
            
            query = query.Where(ja => ja.PersonalFiles!.Any(pf => pfIdHits.Contains(pf.Id)) 
                                      || ja.User!.Cvs!.Any(cv => cv.IsPublic && cvIdHits.Contains(cv.Id)));
        }

        var count = await query.CountAsync(cancellationToken);

        var queryResults = await query
            .OrderByDescending(ja => ja.DateTimeCreatedUtc)
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var jobApplicationDtos = queryResults
            .Select(ja =>
                new JobApplicationForManagersDto(
                    ja.Id,
                    ja.UserId,
                    $"{ja.User!.FirstName} {ja.User!.LastName}",
                    ja.DateTimeCreatedUtc,
                    ja.User!.Cvs!.Select(cv => new CvDto(cv.Id, cv.UserId, cv.SalaryRecord, cv.EmploymentTypeRecord,
                        cv.EducationRecords!, cv.WorkRecords!, cv.Skills!)).FirstOrDefault(),
                    ja.PersonalFiles!.Select(pf =>
                        new PersonalFileInfocardDto(pf.Id, pf.Name, pf.Extension, pf.Size)).ToList(),
                    ja.Status.ToString())
            )
            .ToList();


        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);

        var response = new GetApplicationsForJobIdResponse(jobApplicationDtos, paginationResponse);

        return response;
    }
}