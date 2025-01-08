using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.CompanyClaims.UseCases.UpdateCompanyPermissionIdsForUser;

public class UpdateCompanyPermissionIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) 
    : IRequestHandler<UpdateCompanyPermissionIdsForUserRequest, Result>
{
    public async Task<Result> Handle(UpdateCompanyPermissionIdsForUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new CompanyClaimIdCollectionValidator();
        var validationResult = permissionsValidator.Validate(request.CompanyPermissionIds);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var company = await context.Companies.FindAsync([request.CompanyId], cancellationToken);
        if (company is null)
            return Result.NotFound();
        
        var userExists = await context.UserProfiles.AnyAsync(u => u.Id == request.UserId, cancellationToken);
        if (!userExists)
            return Result.NotFound();
        
        var currentUserClaims = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);

        if (!currentUserClaims.Contains(CompanyClaim.IsAdmin.Id))
            return Result.Forbidden();
        
        var targetUserClaims = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == request.UserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);
        
        if (!targetUserClaims.Contains(CompanyClaim.IsOwner.Id) 
            && !targetUserClaims.Contains(CompanyClaim.IsAdmin.Id))
            return Result.Forbidden();
        
        if (targetUserClaims.Except(currentUserClaims).Any())
            return Result.Forbidden();
        
        company.UserCompanyClaims = request.CompanyPermissionIds.Select(cpId => 
            new UserCompanyClaim(request.UserId, request.CompanyId, cpId)).ToList();
        
        context.Companies.Update(company);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}