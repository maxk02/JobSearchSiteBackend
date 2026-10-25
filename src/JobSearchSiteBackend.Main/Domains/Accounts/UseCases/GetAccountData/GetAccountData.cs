using JobSearchSiteBackend.Main.Domains.Companies;
using JobSearchSiteBackend.Main.Domains.Companies.Dtos;
using JobSearchSiteBackend.Main.Domains.Companies.Persistence;
using JobSearchSiteBackend.Main.Attributes;
using JobSearchSiteBackend.Main.Domains.Accounts.Dtos;
using JobSearchSiteBackend.Main.Domains.UserProfiles.Persistence;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.Auth;
using JobSearchSiteBackend.Main.Services.FileStorage;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Accounts.UseCases.GetAccountData;

public static class GetAccountData
{
    public record Response(AccountDataDto AccountDataDto);

    public static void MapEndpoint(IEndpointRouteBuilder group) =>
        group.MapGet("", Handle)
            .WithMetadata(new AllowUnconfirmedEmailAttribute());

    public static async Task<Results<Ok<Response>, NotFound, ProblemHttpResult>> Handle(
        ICurrentAccountService currentAccountService,
        UserManager<MyIdentityUser> userManager,
        MainDataContext context,
        IFileStorageService fileStorageService,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var identityUser = await userManager.FindByIdAsync(currentAccountId.ToString());
        if (identityUser is null)
            return TypedResults.NotFound();

        var email = identityUser.Email ?? throw new ApplicationException("User email is missing.");

        var userProfileData = await context.UserProfiles
            .Where(u => u.Id == currentAccountId)
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
            .Where(c => c.UserCompanyClaims!.Any(ucc => ucc.UserId == currentAccountId))
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
            currentAccountId,
            email,
            fullName,
            avatarLink,
            companyDtoList);

        return TypedResults.Ok(new Response(accountDataDto));
    }
}