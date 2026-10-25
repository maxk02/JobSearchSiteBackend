using JobSearchSiteBackend.Main.Domains.Accounts.Caching;
using JobSearchSiteBackend.Main.Domains.Accounts.Dtos;
using JobSearchSiteBackend.Main.Domains.Companies;
using JobSearchSiteBackend.Main.Domains.Companies.Dtos;
using JobSearchSiteBackend.Main.Domains.Companies.Persistence;
using JobSearchSiteBackend.Main.Domains.UserProfiles.Persistence;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.Auth;
using JobSearchSiteBackend.Main.Services.Cookies;
using JobSearchSiteBackend.Main.Services.FileStorage;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.LogIn;

public static class LogIn
{
    public record Request(string Email, string Password);
    public record Response(string TokenId, AccountDataDto AccountData);

    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapPost("login", Handle)
            .AllowAnonymous();

    public static async Task<Results<Ok<Response>, UnauthorizedHttpResult, ForbidHttpResult, ProblemHttpResult>> Handle(
        [FromBody] Request request,
        UserManager<MyIdentityUser> userManager,
        MainDataContext context,
        IJwtTokenGenerationService jwtTokenGenerationService,
        IUserSessionCacheRepository sessionCache,
        ICurrentAccountService currentAccountService,
        ICookieService cookieService,
        IFileStorageService fileStorageService,
        CancellationToken cancellationToken)
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
                return TypedResults.Forbid();
            }
        }

        var account = await userManager.FindByEmailAsync(request.Email);
        if (account is null || !account.EmailConfirmed)
            return TypedResults.Unauthorized();

        var isPasswordCorrect = await userManager.CheckPasswordAsync(account, request.Password);
        if (!isPasswordCorrect)
            return TypedResults.Unauthorized();

        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(account);

        var userProfileData = await context.UserProfiles
            .Where(u => u.Id == account.Id)
            .Select(u => new { u.FirstName, u.LastName, u.UserAvatars })
            .SingleOrDefaultAsync(cancellationToken);

        if (userProfileData is null)
            return TypedResults.Problem("User profile not found.", statusCode: StatusCodes.Status500InternalServerError);

        var fullName = $"{userProfileData.FirstName} {userProfileData.LastName}";
        var avatar = userProfileData.UserAvatars?.GetLatestAvailableAvatar();

        string? avatarLink = null;
        if (avatar is not null)
        {
            avatarLink = await fileStorageService.GetDownloadUrlAsync(
                FileStorageBucketName.UserAvatars,
                avatar.GuidIdentifier,
                avatar.Extension,
                cancellationToken);
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
                companyAvatarLink = await fileStorageService.GetDownloadUrlAsync(
                    FileStorageBucketName.CompanyAvatars,
                    companyAvatar.GuidIdentifier,
                    companyAvatar.Extension,
                    cancellationToken);
            }

            companyDtoList.Add(company.ToCompanyDto(companyAvatarLink));
        }

        var accountDataDto = new AccountDataDto(
            account.Id,
            request.Email,
            fullName,
            avatarLink,
            companyDtoList);

        var roles = await userManager.GetRolesAsync(account);
        var accountData = new AccountData(account.Id, isEmailConfirmed, roles);

        var newTokenId = Guid.NewGuid();
        var token = jwtTokenGenerationService.Generate(accountData, newTokenId);

        await sessionCache.AddSessionAsync(account.Id.ToString(), newTokenId.ToString(), DateTime.UtcNow.AddMonths(1));
        cookieService.SetAuthCookie(token);

        return TypedResults.Ok(new Response(newTokenId.ToString(), accountDataDto));
    }
}