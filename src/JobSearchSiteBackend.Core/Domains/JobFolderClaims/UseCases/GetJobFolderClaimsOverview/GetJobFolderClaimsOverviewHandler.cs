using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimsOverview;

public class GetJobFolderClaimsOverviewHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetJobFolderClaimsOverviewQuery, Result<GetJobFolderClaimsOverviewResult>>
{
    public async Task<Result<GetJobFolderClaimsOverviewResult>> Handle(GetJobFolderClaimsOverviewQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var thisFolderOrSomeAncestorWhereUserIsAdminQuery = context.JobFolderRelations
            .Where(jfr => jfr.DescendantId == query.JobFolderId)
            .Where(jfr => jfr.Ancestor!.UserJobFolderClaims!
                .Any(ujfc => ujfc.UserId == currentUserId && ujfc.ClaimId == JobFolderClaim.IsAdmin.Id))
            .Select(jfr => jfr.DescendantId);

        var thisFolderOrSomeAncestorIdWhereUserIsAdmin = await thisFolderOrSomeAncestorWhereUserIsAdminQuery
            .SingleOrDefaultAsync(cancellationToken);

        if (thisFolderOrSomeAncestorIdWhereUserIsAdmin == 0)
        {
            return Result.Forbidden();
        }

        var dbQuery = context.JobFolderRelations
            .Where(jfr => jfr.DescendantId == query.JobFolderId)
            .Where(jfr => jfr.Descendant!.RelationsWhereThisIsDescendant!
                .Any(jfr2 => jfr2.AncestorId == thisFolderOrSomeAncestorIdWhereUserIsAdmin))
            .SelectMany(jfr => jfr.Descendant!.UserJobFolderClaims!,
                (jfr, ujfc) => new JobFolderClaimOverviewDto
                (
                    ujfc.Id,
                    ujfc.UserId,
                    ujfc.User!.FirstName,
                    ujfc.User!.LastName,
                    ujfc.User!.Account!.Email!,
                    ujfc.ClaimId,
                    jfr.Depth > 0,
                    new JobFolderClaimSourceFolderDto(jfr.AncestorId, jfr.Ancestor!.Name!)
                ));
        
        if (!string.IsNullOrEmpty(query.UserQuery))
        {
            dbQuery = dbQuery
                .Where(jfcOverviewDto => jfcOverviewDto.UserFirstName.Contains(query.UserQuery)
                || jfcOverviewDto.UserLastName.Contains(query.UserQuery)
                || jfcOverviewDto.UserEmail.Contains(query.UserQuery));
        }

        if (query.JobFolderClaimIds.Count != 0)
        {
            dbQuery = dbQuery
                .Where(jfcOverviewDto => query.JobFolderClaimIds.Contains(jfcOverviewDto.ClaimId));
        }
        
        dbQuery = dbQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size);
        
        var dbQueryTotalCount = await dbQuery.CountAsync(cancellationToken);
        var dbQueryResult = await dbQuery.ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(query.Page, query.Size, dbQueryTotalCount);
        
        var result = new GetJobFolderClaimsOverviewResult(dbQueryResult, paginationResponse);
        
        return Result.Success(result);
    }
}