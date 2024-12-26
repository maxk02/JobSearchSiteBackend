namespace Core.Domains.JobFolderPermissions;

public interface IJobFolderPermissionRepository
{
    public Task<ICollection<long>> GetPermissionIdsForUserAsync(long userId, long jobFolderId,
        CancellationToken cancellationToken = default);
    
    public Task UpdatePermissionIdsForUserAsync(long userId, long jobFolderId,
        ICollection<long> newPermissionIds, CancellationToken cancellationToken = default);
    
    public Task<bool> HasPermissionIdAsync(long userId, long jobFolderId,
        long permissionId, CancellationToken cancellationToken = default);
}