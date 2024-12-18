namespace Core.Domains.CompanyPermissions;

public interface ICompanyPermissionRepository
{
    public Task UpdatePermissionsForUserAsync(long userId, long companyId,
        ICollection<long> newPermissionIds, CancellationToken cancellationToken = default);
}