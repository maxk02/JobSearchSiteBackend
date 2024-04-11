using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface ITagRepository : IBaseRepository<Tag>
{
    void AttachJob(long tagId, long jobId);
    void DetachJob(long tagId, long jobId);
    Task<Job> GetAllJobsPaginatedResults(long tagId, CancellationToken cancellationToken); //
    
    Task<UserTagPermissionSet?> GetUserPermissionSet(long tagId, long userId, CancellationToken cancellationToken);
    // Task<IList<UserTagPermissionSet>> GetAllUserPermissionSets(long tagId, CancellationToken cancellationToken);
    void AddUserPermissionSet(UserTagPermissionSet permissionSet);
    void UpdateUserPermissionSet(UserTagPermissionSet permissionSet);
    void RemoveUserPermissionSet(long tagId, long userId);
}