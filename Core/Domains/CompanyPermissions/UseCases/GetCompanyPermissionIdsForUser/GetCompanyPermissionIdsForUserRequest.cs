namespace Core.Domains.CompanyPermissions.UseCases.GetCompanyPermissionIdsForUser;

public record GetCompanyPermissionIdsForUserRequest(long UserId, long CompanyId);