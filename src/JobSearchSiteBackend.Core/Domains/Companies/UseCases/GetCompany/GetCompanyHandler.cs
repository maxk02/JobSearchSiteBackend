using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;

public class GetCompanyHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IFileStorageService fileStorageService)
    : IRequestHandler<GetCompanyQuery, Result<GetCompanyResult>>
{
    public async Task<Result<GetCompanyResult>> Handle(GetCompanyQuery query,
        CancellationToken cancellationToken = default)
    {
        var company = await context.Companies
            .Include(c => c.CompanyAvatars)
            .Where(c => c.Id == query.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (company is null)
            return Result<GetCompanyResult>.NotFound();

        if (!company.IsPublic)
        {
            var currentUserId = currentAccountService.GetId();

            if (currentUserId is null)
                return Result<GetCompanyResult>.Forbidden("Requested company profile is private.");
            
            var canEditProfile = await context.UserCompanyClaims
                .Where(
                    ucp => ucp.UserId == currentUserId 
                    && ucp.CompanyId == company.Id 
                    && ucp.ClaimId == CompanyClaim.CanEditProfile.Id)
                .AnyAsync(cancellationToken);

            if (!canEditProfile)
                return Result<GetCompanyResult>.Forbidden("Requested company profile is private.");
        }
        
        var avatar = company.CompanyAvatars?.GetLatestAvailableAvatar();

        string? avatarLink = null;
        
        if (avatar is not null)
        {
            avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.UserAvatars, 
                avatar.GuidIdentifier, avatar.Extension, cancellationToken);
        }
        
        var companyInfoDto = company.ToCompanyDto(avatarLink);

        var result = new GetCompanyResult(companyInfoDto);
        
        return Result.Success(result);
    }
}