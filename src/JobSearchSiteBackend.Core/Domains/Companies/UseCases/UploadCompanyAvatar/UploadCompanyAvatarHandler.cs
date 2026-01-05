using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;

public class UploadCompanyAvatarHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<UploadCompanyAvatarCommand, Result<UploadCompanyAvatarResult>>
{
    public async Task<Result<UploadCompanyAvatarResult>> Handle(UploadCompanyAvatarCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var hasPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.CompanyId == command.CompanyId
                          && ucc.UserId == currentUserId
                          && ucc.ClaimId == CompanyClaim.IsAdmin.Id)
            .AnyAsync(cancellationToken);
            
        if (!hasPermission)
            return Result.Forbidden();
        
        var existingAvatars = context.CompanyAvatars.Where(a => a.CompanyId == command.CompanyId).ToList();
        foreach (var existingAvatar in existingAvatars)
        {
            existingAvatar.IsDeleted = true;
        }
        
        var companyAvatar = new CompanyAvatar(command.CompanyId, command.Extension, command.Size);
        
        context.CompanyAvatars.Add(companyAvatar);
        context.CompanyAvatars.UpdateRange(existingAvatars);
        await context.SaveChangesAsync(cancellationToken);
        
        return new UploadCompanyAvatarResult(companyAvatar.Id);
    }
}