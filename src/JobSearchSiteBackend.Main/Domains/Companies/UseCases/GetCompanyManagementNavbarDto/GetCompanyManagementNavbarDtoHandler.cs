using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Companies.Dtos;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.Auth;
using JobSearchSiteBackend.Main.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;

public class GetCompanyManagementNavbarDtoHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    MainDataContext context) : IRequestHandler<GetCompanyManagementNavbarDtoQuery, Result<GetCompanyManagementNavbarDtoResult>>
{
    public async Task<Result<GetCompanyManagementNavbarDtoResult>> Handle(GetCompanyManagementNavbarDtoQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        var company = await context.Companies
            .AsNoTracking()
            .Include(c => c.Country)
            .Include(c => c.UserCompanyClaims!.Where(ucc => ucc.UserId == currentAccountId))
            .Include(c => c.CompanyAvatars!.Where(a => !a.IsDeleted && a.IsUploadedSuccessfully).OrderBy(a => a.DateTimeUpdatedUtc))
            .Where(c => c.Id == query.CompanyId)
            .FirstOrDefaultAsync(cancellationToken);

        if (company is null)
        {
            return Result.NotFound();
        }
        
        var lastAvatar = company.CompanyAvatars!.LastOrDefault();

        string? companyLogoLink = null;
            
        if (lastAvatar is not null)
        {
            companyLogoLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars,
                lastAvatar.GuidIdentifier, lastAvatar.Extension, cancellationToken);
        }

        var companyManagementDetailedDto = new CompanyManagementDetailedDto(
            company.Id,
            company.Name,
            company.Description,
            company.CountryId,
            companyLogoLink,
            company.UserCompanyClaims!.Select(x => x.ClaimId).ToList(),
            company.CountrySpecificFieldsJson,
            company.IsVerified
            );

        var result = new GetCompanyManagementNavbarDtoResult(companyManagementDetailedDto);

        return Result.Success(result);
    }
}