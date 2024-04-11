using Application.DTOs.SortFilterDTOs;
using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<IList<Company>> GetPaginatedResults(CompanySortFilterDto filterDto, CancellationToken cancellationToken);
    
    void AttachCountry(long companyId, long countryId);
    void DetachCountry(long companyId, long countryId);
    Task<IList<Country>> GetCountries(long companyId, CancellationToken cancellationToken);
    
    Task<UserCompanyPermissionSet?> GetUserPermissionSet(long companyId, long userId, CancellationToken cancellationToken);
    Task<IList<UserCompanyPermissionSet>> GetAllUserPermissionSets(long companyId, CancellationToken cancellationToken);
    void AddUserPermissionSet(UserCompanyPermissionSet permissionSet);
    void UpdateUserPermissionSet(UserCompanyPermissionSet permissionSet);
    void RemoveUserPermissionSet(long companyId, long userId);
}