namespace Core.Domains.CompanyPermissions;

public interface ICompanyPermissionRepository
{
    public Task UpdatePermissionsForUserAsync(long userId, ICollection<CompanyPermission> permissions);
}