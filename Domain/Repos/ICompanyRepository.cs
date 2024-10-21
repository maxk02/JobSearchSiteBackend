using Domain.Entities;
using Domain.Entities.Companies;
using Domain.Entities.Users;
using Domain.Shared.Repos;

namespace Domain.Repos;

public interface ICompanyRepository : IBaseRepository<Company>
{
    // Task<IList<Company>> GetPaginatedResults(CompanySortFilterDto filterDto, CancellationToken cancellationToken);
    
    Task<UserCompanyPermissionSet?> GetUserPermissionSet(long companyId, long userId, CancellationToken cancellationToken);
    Task<IList<UserCompanyPermissionSet>> GetAllUserPermissionSets(long companyId, CancellationToken cancellationToken);
    void AddUserPermissionSet(UserCompanyPermissionSet permissionSet);
    void UpdateUserPermissionSet(UserCompanyPermissionSet permissionSet);
    void RemoveUserPermissionSet(long companyId, long userId);
}