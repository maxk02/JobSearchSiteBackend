using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.CompanyPermissions.UseCases.UpdateCompanyPermissionIdsForUser;

public class UpdateCompanyPermissionIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    ICompanyPermissionRepository companyPermissionRepository) 
    : IRequestHandler<UpdateCompanyPermissionIdsForUserRequest, Result>
{
    public async Task<Result> Handle(UpdateCompanyPermissionIdsForUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new CompanyPermissionIdCollectionValidator();
        var validationResult = permissionsValidator.Validate(request.CompanyPermissionIds);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var currentUserPermissions = await companyPermissionRepository
            .GetPermissionIdsForUserAsync(currentUserId, request.CompanyId, cancellationToken);

        if (!currentUserPermissions.Contains(CompanyPermission.IsAdmin.Id))
            return Result.Forbidden("Current user is not a company admin.");
        
        var targetUserPermissions = await companyPermissionRepository
            .GetPermissionIdsForUserAsync(request.UserId, request.CompanyId, cancellationToken);
        
        if (targetUserPermissions.Contains(CompanyPermission.IsOwner.Id) || targetUserPermissions.Contains(CompanyPermission.IsAdmin.Id))
            return Result.Forbidden("Insufficient permissions for update of permissions of target user.");
        
        if (targetUserPermissions.Except(currentUserPermissions).Any())
            return Result.Forbidden("Insufficient permissions for update of permissions of target user.");
        
        await companyPermissionRepository.UpdatePermissionIdsForUserAsync(currentUserId, request.CompanyId,
            request.CompanyPermissionIds, cancellationToken);
        
        return Result.Success();
    }
}