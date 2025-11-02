using System.Transactions;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Search;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;

public class UploadCompanyAvatarHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<UploadCompanyAvatarRequest, Result<UploadCompanyAvatarResponse>>
{
    public async Task<Result<UploadCompanyAvatarResponse>> Handle(UploadCompanyAvatarRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var hasPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.CompanyId == request.CompanyId
                          && ucc.UserId == currentUserId
                          && ucc.ClaimId == CompanyClaim.IsAdmin.Id)
            .AnyAsync(cancellationToken);
            
        if (!hasPermission)
            return Result.Forbidden();
        
        var existingAvatars = context.CompanyAvatars.Where(a => a.CompanyId == request.CompanyId).ToList();
        foreach (var existingAvatar in existingAvatars)
        {
            existingAvatar.IsDeleted = true;
        }
        
        var companyAvatar = new CompanyAvatar(request.CompanyId, request.Extension, request.Size);
        
        context.CompanyAvatars.Add(companyAvatar);
        context.CompanyAvatars.UpdateRange(existingAvatars);
        await context.SaveChangesAsync(cancellationToken);
        
        return new UploadCompanyAvatarResponse(companyAvatar.Id);
    }
}