using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domains.Accounts.Dtos;
using Core.Domains.Companies.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.Accounts.UseCases.LogIn;

public class LogInHandler(UserManager<MyIdentityUser> userManager,
    MainDataContext context,
    IJwtGenerationService jwtGenerationService,
    IMapper mapper) 
    : IRequestHandler<LogInRequest, Result<LogInResponse>>
{
    public async Task<Result<LogInResponse>> Handle(LogInRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<LogInResponse>.NotFound();

        var isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordCorrect)
            return Result<LogInResponse>.Unauthorized();

        var roles = await userManager.GetRolesAsync(user);

        var accountData = new AccountData(user.Id, roles);
        
        var newTokenId = Guid.NewGuid();
        
        var token = jwtGenerationService.Generate(accountData, newTokenId);
        
        var newUserSession = new UserSession(newTokenId.ToString(), user.Id, DateTime.UtcNow,
            DateTime.UtcNow.Add(TimeSpan.FromDays(30)));
        
        context.UserSessions.Add(newUserSession);
        await context.SaveChangesAsync(cancellationToken);

        var fullName = await context.UserProfiles
            .Where(u => u.Id == user.Id)
            .Select(u => $"{u.FirstName} {u.LastName}")
            .SingleOrDefaultAsync(cancellationToken);
        
        var companyInfoDtos = await context.Companies
            .Where(c => c.UserCompanyClaims!.Any(ucc => ucc.UserId == user.Id))
            .Distinct()
            .ProjectTo<CompanyInfoDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var companyInfoDtosFromFolders = await context.Companies
            .Where(c => c.JobFolders!.Any(jf => jf.UserJobFolderClaims!.Any(ujfc => ujfc.UserId == user.Id)))
            .Distinct()
            .ProjectTo<CompanyInfoDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var combinedCompanyInfoDtos = companyInfoDtos
            .Concat(companyInfoDtosFromFolders)
            .DistinctBy(c => c.Id)
            .ToList();

        var accountDataDto = new AccountDataDto(user.Id, user.Email ?? "", fullName ?? "", combinedCompanyInfoDtos);

        return new LogInResponse(accountDataDto, token, newTokenId.ToString());
    }
}