using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    void AttachLocation(long userId, long locationId);
    void DetachLocation(long userId, long locationId);
    Task<IList<Location>> GetAllLocations(long userId);
    
    void AttachCategory(long userId, long categoryId);
    void DetachCategory(long userId, long categoryId);
    Task<IList<Category>> GetAllCategories(long userId);
    
    void AddJobBookmark(long userId, long jobId);
    void RemoveJobBookmark(long userId, long jobId);
    Task<IList<Job>> GetJobBookmarksPaginatedResults(long userId); //
    
    void AddCompanyBookmark(long userId, long companyId);
    void RemoveCompanyBookmark(long userId, long companyId);
    Task<IList<Company>> GetCompanyBookmarksPaginatedResults(long userId); //
    
    Task<UserTagPermissionSet?> GetTagPermissionSet(long userId, long tagId, CancellationToken cancellationToken);
    // Task<IList<UserTagPermissionSet>> GetAllTagPermissionSets(long userId, CancellationToken cancellationToken);
    void AddTagPermissionSet(UserTagPermissionSet permissionSet);
    void UpdateTagPermissionSet(UserTagPermissionSet permissionSet);
    void RemoveTagPermissionSet(long userId, long tagId);
    
    Task<UserJobPermissionSet?> GetJobPermissionSet(long userId, long jobId, CancellationToken cancellationToken);
    // Task<IList<UserJobPermissionSet>> GetAllJobPermissionSets(long userId, CancellationToken cancellationToken);
    void AddJobPermissionSet(UserJobPermissionSet permissionSet);
    void UpdateJobPermissionSet(UserJobPermissionSet permissionSet);
    void RemoveJobPermissionSet(long userId, long jobId);
    
    Task<UserCompanyPermissionSet?> GetCompanyPermissionSet(long userId, long companyId, CancellationToken cancellationToken);
    // Task<IList<UserCompanyPermissionSet>> GetAllCompanyPermissionSets(long userId, CancellationToken cancellationToken);
    void AddCompanyPermissionSet(UserCompanyPermissionSet permissionSet);
    void UpdateCompanyPermissionSet(UserCompanyPermissionSet permissionSet);
    void RemoveCompanyPermissionSet(long userId, long companyId);
}