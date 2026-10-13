using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.UserProfiles;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployees;

public class GetCompanyEmployeesHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    MainDataContext context)
    : IRequestHandler<GetCompanyEmployeesQuery, Result<GetCompanyEmployeesResult>>
{
    public async Task<Result<GetCompanyEmployeesResult>> Handle(GetCompanyEmployeesQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var hasPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.UserId == currentUserId
                          && ucc.CompanyId == query.CompanyId
                          && ucc.ClaimId == CompanyClaim.IsAdmin.Id)
            .AnyAsync(cancellationToken);

        if (!hasPermission)
        {
            return Result.Forbidden();
        }

        var dbQuery = context.Companies
            .AsNoTracking()
            .Where(c => c.Id == query.CompanyId)
            .SelectMany(c => c.Employees!);

        if (!string.IsNullOrWhiteSpace(query.Query))
        {
            var lowerTextQuery = query.Query.ToLower();

            dbQuery = dbQuery
                .Where(employee => employee.Account!.Email!.ToLower().Contains(lowerTextQuery)
                || employee.FirstName.ToLower().Contains(lowerTextQuery)
                || employee.LastName.Contains(lowerTextQuery));
        }

        var count = await dbQuery.CountAsync(cancellationToken);
        
        var dbQueryWithFiltersNJoins = dbQuery
            .Include(employee => employee.Account)
            .Include(employee => employee.UserAvatars!
                .Where(a => !a.IsDeleted && a.IsUploadedSuccessfully)
                .OrderBy(a => a.DateTimeUpdatedUtc))
            .Take(query.Size)
            .Skip((query.Page - 1) * query.Size);

        var companyEmployees = await dbQueryWithFiltersNJoins
            .ToListAsync(cancellationToken);

        List<CompanyEmployeeDto> companyEmployeeDtos = [];

        foreach (var companyEmployee in companyEmployees)
        {
            var lastAvatar = companyEmployee.UserAvatars!
                .LastOrDefault();

            string? url = null;
            
            if (lastAvatar is not null)
            {
                url = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.UserAvatars,
                    lastAvatar.GuidIdentifier, lastAvatar.Extension, cancellationToken);
            }
            
            companyEmployeeDtos.Add(new CompanyEmployeeDto(
                companyEmployee.Id,
                companyEmployee.Account!.Email!,
                $"{companyEmployee.FirstName} {companyEmployee.LastName}",
                url));
        }
        
        var paginationResponse = new PaginationResponse(query.Page, query.Size, count);

        var response = new GetCompanyEmployeesResult(companyEmployeeDtos, paginationResponse);
        
        return Result.Success(response);
    }
}