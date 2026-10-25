// using Ardalis.Result;
// using AutoMapper;
// using AutoMapper.QueryableExtensions;
// using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
// using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
// using JobSearchSiteBackend.Main.Domains.JobFolderClaims;
// using JobSearchSiteBackend.Main.Domains.JobFolders.Dtos;
// using JobSearchSiteBackend.Main.Domains.JobFolders.Persistence;
// using JobSearchSiteBackend.Main.Domains.Jobs;
// using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;
// using JobSearchSiteBackend.Main.Domains.Locations;
// using JobSearchSiteBackend.Main.Persistence;
// using JobSearchSiteBackend.Main.Services.Auth;
// using JobSearchSiteBackend.Main.Services.Caching;
// using Microsoft.EntityFrameworkCore;

// namespace JobSearchSiteBackend.Main.Domains.JobFolders.UseCases.GetFolderJobs;

// public class GetFolderJobsHandler(
//     ICurrentAccountService currentAccountService,
//     MainDataContext context,
//     ICompanyLastVisitedFoldersCacheRepository cacheRepo) : IRequestHandler<GetFolderJobsQuery, Result<GetFolderJobsResult>>
// {
//     public async Task<Result<GetFolderJobsResult>> Handle(GetFolderJobsQuery query,
//         CancellationToken cancellationToken = default)
//     {
//         var currentUserId = currentAccountService.GetIdOrThrow();

//         var jobFolder = await context.JobFolders.FindAsync([query.Id], cancellationToken);
//         if (jobFolder is null)
//             return Result<GetFolderJobsResult>.NotFound();

//         var hasReadClaim = await context.JobFolderRelations
//             .GetThisOrAncestorWhereUserHasClaim(query.Id, currentUserId,
//                 JobFolderClaim.CanReadJobs.Id)
//             .AnyAsync(cancellationToken);

//         if (!hasReadClaim)
//             return Result<GetFolderJobsResult>.Forbidden();

//         await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
//             jobFolder.CompanyId.ToString(), jobFolder.Id.ToString());
        
//         var dbBasicQuery = context.Jobs
//             .Where(job => job.JobFolderId == query.Id)
//             .Where(job => !job.IsDeleted);
        
//         if (!string.IsNullOrEmpty(query.Query))
//         {
//             dbBasicQuery = dbBasicQuery
//                 .Where(job => job.Title.ToLower().Contains(query.Query.ToLower()));
//         }
        
//         var count = await dbBasicQuery.CountAsync(cancellationToken);
        
//         if (query.Page is > 0 && query.Size is > 0)
//         {
//             dbBasicQuery = dbBasicQuery
//                 .Skip((query.Page - 1) * query.Size)
//                 .Take(query.Size);
//         }
        
//         var dbQuery = dbBasicQuery
//             .OrderByDescending(job => job.DateTimePublishedUtc)
//             .Select(job => new
//             {
//                 JobId = job.Id,
//                 Locations = job.Locations!,
//                 Title = job.Title,
//                 DateTimePublishedUtc = job.DateTimePublishedUtc,
//                 DateTimeExpiringUtc = job.DateTimeExpiringUtc,
//                 SalaryInfo = job.SalaryInfo,
//                 EmploymentOptionIds = job.EmploymentOptions!.Select(eo => eo.Id),
//                 ContractTypeIds = job.JobContractTypes!.Select(ct => ct.Id),
//                 IsBookmarked = job.UserJobBookmarks!.Any(ujb => ujb.UserId == currentUserId),
//                 IsPublic = job.IsPublic
//             });

//         var jobObjects = await dbQuery.ToListAsync(cancellationToken);

//         var jobManagementCardDtos = jobObjects
//             .Select(jo => new JobManagementCardDto(
//                 jo.JobId,
//                 jo.Locations.Select(l => l.ToLocationDto()).ToList(),
//                 jo.Title,
//                 jo.DateTimePublishedUtc,
//                 jo.DateTimeExpiringUtc,
//                 jo.SalaryInfo?.ToJobSalaryInfoDto(),
//                 jo.EmploymentOptionIds.ToList(),
//                 jo.ContractTypeIds.ToList(),
//                 jo.IsBookmarked,
//                 jo.IsPublic)
//             )
//             .ToList();

//         var paginationResponse = new PaginationResponse(query.Page, query.Size, count);

//         var result = new GetFolderJobsResult(jobManagementCardDtos, paginationResponse); 
        
//         return Result.Success(result);
//     }
// }