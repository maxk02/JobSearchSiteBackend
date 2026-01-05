using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains.Accounts.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.Cookies;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;

public class LogInHandler(UserManager<MyIdentityUser> userManager,
    MainDataContext context,
    IJwtTokenGenerationService jwtTokenGenerationService,
    IUserSessionCacheRepository sessionCache,
    ICurrentAccountService currentAccountService,
    ICookieService cookieService,
    IFileStorageService fileStorageService) 
    : IRequestHandler<LogInCommand, Result<LogInResult>>
{
    public async Task<Result<LogInResult>> Handle(LogInCommand command, CancellationToken cancellationToken = default)
    {
        if (currentAccountService.IsLoggedIn())
        {
            var tokenId = currentAccountService.GetTokenIdentifierOrThrow();
            var userId = currentAccountService.GetIdOrThrow();

            var expirationUtc = await sessionCache.GetSessionExpirationUtcAsync(userId.ToString(), tokenId); 
            
            if (expirationUtc is null || expirationUtc <= DateTime.UtcNow)
            {
                cookieService.RemoveAuthCookie();
            }
            else
            {
                return Result.Forbidden();
            }
        }
        
        var account = await userManager.FindByEmailAsync(command.Email);

        if (account is null)
            return Result.NotFound();

        var isPasswordCorrect = await userManager.CheckPasswordAsync(account, command.Password);

        if (!isPasswordCorrect)
            return Result.Unauthorized();
        
        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(account);

        var userProfileData = await context.UserProfiles
            .Where(u => u.Id == account.Id)
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
            .Where(c => c.UserCompanyClaims!.Any(ucc => ucc.UserId == account.Id))
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
        
        var accountDataDto = new AccountDataDto(account.Id, command.Email,
            fullName, avatarLink, companyDtoList);
        
        // token generation + session adding
        
        var roles = await userManager.GetRolesAsync(account);

        var accountData = new AccountData(account.Id, isEmailConfirmed, roles);
        
        var newTokenId = Guid.NewGuid();
        
        var token = jwtTokenGenerationService.Generate(accountData, newTokenId);

        await sessionCache.AddSessionAsync(account.Id.ToString(), newTokenId.ToString(), DateTime.UtcNow.AddMonths(1));
        
        cookieService.SetAuthCookie(token);

        return new LogInResult(newTokenId.ToString(), accountDataDto);
    }
}