using Core.Domains.CompanyPermissions.UseCases.GetCompanyPermissionIdsForUser;
using Core.Domains.CompanyPermissions.UseCases.UpdateCompanyPermissionIdsForUser;
using Core.Services.Auth.Authentication;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.CompanyPermissions;

public class CompanyPermissionService(ICurrentAccountService currentAccountService,
    ICompanyPermissionRepository companyPermissionRepository) : ICompanyPermissionService
{
    public async Task<Result<ICollection<long>>> GetCompanyPermissionIdsForUserAsync(GetCompanyPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (currentUserId != request.UserId)
        {
            var isAdmin = await companyPermissionRepository.HasPermissionIdAsync(currentUserId,
                request.CompanyId, CompanyPermission.IsAdmin.Id, cancellationToken);
            
            if (!isAdmin) 
                return Result<ICollection<long>>.Forbidden();
        }
        
        var permissions = await companyPermissionRepository
            .GetPermissionIdsForUserAsync(request.UserId, request.CompanyId, cancellationToken);

        return Result<ICollection<long>>.Success(permissions);
    }
    
    public async Task<Result> UpdateCompanyPermissionIdsForUserAsync(UpdateCompanyPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default)
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