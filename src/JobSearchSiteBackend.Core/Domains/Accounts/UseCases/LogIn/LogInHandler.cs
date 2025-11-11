using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains.Accounts.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.Cookies;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;

public class LogInHandler(UserManager<MyIdentityUser> userManager,
    MainDataContext context,
    IJwtTokenGenerationService jwtTokenGenerationService,
    IMapper mapper,
    IUserSessionCacheRepository sessionCache,
    ICurrentAccountService currentAccountService,
    ICookieService cookieService) 
    : IRequestHandler<LogInRequest, Result<LogInResponse>>
{
    public async Task<Result<LogInResponse>> Handle(LogInRequest request, CancellationToken cancellationToken = default)
    {
        if (currentAccountService.IsLoggedIn())
        {
            return Result.Forbidden();
        }
        
        var account = await userManager.FindByEmailAsync(request.Email);

        if (account is null)
            return Result<LogInResponse>.NotFound();

        var isPasswordCorrect = await userManager.CheckPasswordAsync(account, request.Password);

        if (!isPasswordCorrect)
            return Result<LogInResponse>.Unauthorized();
        
        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(account);

        var userProfileData = await context.UserProfiles
            .Where(u => u.Id == account.Id)
            .Select(u => new { u.FirstName, u.LastName })
            .SingleOrDefaultAsync(cancellationToken);

        var fullName = userProfileData is not null ? $"{userProfileData.FirstName} {userProfileData.LastName}" : null;
        var avatarLink = ""; // todo
        
        var companyInfoDtos = await context.Companies
            .Where(c => c.UserCompanyClaims!.Any(ucc => ucc.UserId == account.Id))
            .Distinct()
            .ProjectTo<CompanyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var companyInfoDtosFromFolders = await context.Companies
            .Where(c => c.JobFolders!.Any(jf => jf.UserJobFolderClaims!.Any(ujfc => ujfc.UserId == account.Id)))
            .Distinct()
            .ProjectTo<CompanyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var combinedCompanyInfoDtos = companyInfoDtos
            .Concat(companyInfoDtosFromFolders)
            .DistinctBy(c => c.Id)
            .ToList();
        
        var accountDataDto = new AccountDataDto(account.Id, request.Email, fullName, avatarLink, combinedCompanyInfoDtos); // todo avatar
        
        
        // token generation + session adding
        
        var roles = await userManager.GetRolesAsync(account);

        var accountData = new AccountData(account.Id, isEmailConfirmed, roles);
        
        var newTokenId = Guid.NewGuid();
        
        var token = jwtTokenGenerationService.Generate(accountData, newTokenId);

        await sessionCache.AddSessionAsync(account.Id.ToString(), newTokenId.ToString(), DateTime.UtcNow.AddMonths(1));
        
        cookieService.SetAuthCookie(token);

        return new LogInResponse(newTokenId.ToString(), accountDataDto);
    }
}