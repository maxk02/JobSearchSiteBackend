namespace Core.Domains.CompanyPermissions;

public interface ICompanyPermissionRepository
{
    public Task<ICollection<long>> GetPermissionIdsForUserAsync(long userId, long companyId,
        CancellationToken cancellationToken = default);
    
    public Task UpdatePermissionIdsForUserAsync(long userId, long companyId,
        ICollection<long> newPermissionIds, CancellationToken cancellationToken = default);
    
    public Task<bool> HasPermissionIdAsync(long userId, long companyId,
        long permissionId, CancellationToken cancellationToken = default);
}