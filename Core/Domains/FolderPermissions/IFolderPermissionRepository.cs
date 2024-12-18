namespace Core.Domains.FolderPermissions;

public interface IFolderPermissionRepository
{
    public Task UpdatePermissionsForUserAsync(long userId, long folderId,
        ICollection<long> newPermissionIds, CancellationToken cancellationToken = default);
}