namespace Core.Domains.FolderPermissions;

public interface IFolderPermissionRepository
{
    public Task<ICollection<long>> GetPermissionIdsForUserAsync(long userId, long folderId,
        CancellationToken cancellationToken = default);
    
    public Task UpdatePermissionIdsForUserAsync(long userId, long folderId,
        ICollection<long> newPermissionIds, CancellationToken cancellationToken = default);
    
    public Task<bool> HasPermissionIdAsync(long userId, long folderId,
        long permissionId, CancellationToken cancellationToken = default);
}