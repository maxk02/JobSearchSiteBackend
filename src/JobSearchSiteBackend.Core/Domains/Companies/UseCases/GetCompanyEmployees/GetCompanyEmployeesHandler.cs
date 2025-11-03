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
    IFileStorageService  fileStorageService,
    MainDataContext context,
    IMapper mapper)
    : IRequestHandler<GetCompanyEmployeesRequest, Result<GetCompanyEmployeesResponse>>
{
    public async Task<Result<GetCompanyEmployeesResponse>> Handle(GetCompanyEmployeesRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var hasPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.UserId == currentUserId
                          && ucc.CompanyId == request.CompanyId
                          && ucc.ClaimId == CompanyClaim.IsAdmin.Id)
            .AnyAsync(cancellationToken);

        if (!hasPermission)
        {
            return Result.Forbidden();
        }

        var companyEmployeesGeneralQuery = context.Companies
            .AsNoTracking()
            .Where(c => c.Id == request.CompanyId)
            .SelectMany(c => c.Employees!);
        
        var count = await companyEmployeesGeneralQuery.CountAsync(cancellationToken);
        
        var companyEmployeesQueryWithFiltersNJoins = companyEmployeesGeneralQuery
            .Include(employee => employee.Account)
            .Include(employee => employee.UserAvatars!
                .Where(a => !a.IsDeleted && a.IsUploadedSuccessfully)
                .OrderBy(a => a.DateTimeUpdatedUtc))
            .Take(request.Size)
            .Skip((request.Page - 1) * request.Size);

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            companyEmployeesQueryWithFiltersNJoins = companyEmployeesQueryWithFiltersNJoins
                .Where(employee => employee.Account!.Email!.Contains(request.Query)
                || employee.FirstName.Contains(request.Query)
                || employee.LastName.Contains(request.Query));
        }

        var companyEmployees = await companyEmployeesQueryWithFiltersNJoins
            .ToListAsync(cancellationToken);

        List<CompanyEmployeeDto> companyEmployeeDtos = [];

        foreach (var companyEmployee in companyEmployees)
        {
            var lastAvatar = companyEmployee.UserAvatars!.LastOrDefault();

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
        
        var paginationResponse = new PaginationResponse(request.Page, request.Size, count);

        var response = new GetCompanyEmployeesResponse(companyEmployeeDtos, paginationResponse);
        
        return Result.Success(response);
    }
}