using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Accounts.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetAccountData;

public class GetAccountDataHandler(
    ICurrentAccountService currentAccountService,
    UserManager<MyIdentityUser> userManager,
    MainDataContext context,
    IFileStorageService fileStorageService) : IRequestHandler<GetAccountDataQuery, Result<GetAccountDataResult>>
{
    public async Task<Result<GetAccountDataResult>> Handle(GetAccountDataQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var identityUser = await userManager.FindByIdAsync(currentAccountId.ToString());

        if (identityUser is null)
        {
            return Result.NotFound();
        }
        
        var isEmailConfirmed = identityUser.EmailConfirmed;
        var email = identityUser.Email ?? throw new ApplicationException();

        var userProfileData = await context.UserProfiles
            .Where(u => u.Id == currentAccountId)
            .Select(u => new { u.FirstName, u.LastName, u.UserAvatars })
            .SingleOrDefaultAsync(cancellationToken);

        if (userProfileData is null)
        {
            return Result.Error();
        }

        var fullName = $"{userProfileData.FirstName} {userProfileData.LastName}";

        var avatar = userProfileData.UserAvatars?.GetLatestAvailableAvatar();
        
        string? avatarLink = null;

        if (avatar is not null)
        {
            avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.UserAvatars, 
                avatar.GuidIdentifier, avatar.Extension, cancellationToken);
        }
        
        var companiesWhereHasPermissions = await context.Companies
            .Include(c => c.CompanyAvatars)
            .Where(c => c.UserCompanyClaims!.Any(ucc => ucc.UserId == currentAccountId))
            .ToListAsync(cancellationToken);

        List<CompanyDto> companyDtoList = [];

        foreach (var company in companiesWhereHasPermissions)
        {
            string? companyAvatarLink = null;
            
            var companyAvatar = company.CompanyAvatars?.GetLatestAvailableAvatar();

            if (companyAvatar is not null)
            {
                companyAvatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars, 
                    companyAvatar.GuidIdentifier, companyAvatar.Extension, cancellationToken);
            }
            
            companyDtoList.Add(company.ToCompanyDto(companyAvatarLink));
        }
        
        var accountDataDto = new AccountDataDto(currentAccountId, email,
            fullName, avatarLink, companyDtoList);
        
        var result = new GetAccountDataResult(accountDataDto);

        return Result.Success(result);
    }
}