using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyPermissions.UserCompanyPermissions;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.CompanyPermissions.UseCases.UpdateCompanyPermissionIdsForUser;

public class UpdateCompanyPermissionIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) 
    : IRequestHandler<UpdateCompanyPermissionIdsForUserRequest, Result>
{
    public async Task<Result> Handle(UpdateCompanyPermissionIdsForUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new CompanyPermissionIdCollectionValidator();
        var validationResult = permissionsValidator.Validate(request.CompanyPermissionIds);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var company = await context.Companies.FindAsync([request.CompanyId], cancellationToken);
        if (company is null)
            return Result.NotFound();
        
        var userExists = await context.UserProfiles.AnyAsync(u => u.Id == request.UserId, cancellationToken);
        if (!userExists)
            return Result.NotFound();
        
        var currentUserPermissions = await context.UserCompanyPermissions
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);

        if (!currentUserPermissions.Contains(CompanyPermission.IsAdmin.Id))
            return Result.Forbidden();
        
        var targetUserPermissions = await context.UserCompanyPermissions
            .Where(ucp => ucp.UserId == request.UserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);
        
        if (!targetUserPermissions.Contains(CompanyPermission.IsOwner.Id) 
            && !targetUserPermissions.Contains(CompanyPermission.IsAdmin.Id))
            return Result.Forbidden();
        
        if (targetUserPermissions.Except(currentUserPermissions).Any())
            return Result.Forbidden();
        
        company.UserCompanyPermissions = request.CompanyPermissionIds.Select(cpId => 
            new UserCompanyPermission(request.UserId, request.CompanyId, cpId)).ToList();
        
        context.Companies.Update(company);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}