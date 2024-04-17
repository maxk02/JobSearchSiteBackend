using Application.DTOs.SortFilterDTOs;
using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<IList<Company>> GetPaginatedResults(CompanySortFilterDto filterDto, CancellationToken cancellationToken);
    
    Task<UserCompanyPermissionSet?> GetUserPermissionSet(long companyId, long userId, CancellationToken cancellationToken);
    Task<IList<UserCompanyPermissionSet>> GetAllUserPermissionSets(long companyId, CancellationToken cancellationToken);
    void AddUserPermissionSet(UserCompanyPermissionSet permissionSet);
    void UpdateUserPermissionSet(UserCompanyPermissionSet permissionSet);
    void RemoveUserPermissionSet(long companyId, long userId);
}