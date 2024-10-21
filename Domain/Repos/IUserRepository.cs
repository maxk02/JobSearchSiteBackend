using Domain.Entities;
using Domain.Entities.Categories;
using Domain.Entities.Companies;
using Domain.Entities.Jobs;
using Domain.Entities.Locations;
using Domain.Entities.Users;
using Domain.Shared.Repos;

namespace Domain.Repos;

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
    
    Task<UserCompanyPermissionSet?> GetCompanyPermissionSet(long userId, long companyId, CancellationToken cancellationToken);
    // Task<IList<UserCompanyPermissionSet>> GetAllCompanyPermissionSets(long userId, CancellationToken cancellationToken);
    void AddCompanyPermissionSet(UserCompanyPermissionSet permissionSet);
    void UpdateCompanyPermissionSet(UserCompanyPermissionSet permissionSet);
    void RemoveCompanyPermissionSet(long userId, long companyId);
}