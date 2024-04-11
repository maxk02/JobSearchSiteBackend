using Application.DTOs.RepositoryDTOs;
using Application.DTOs.SortFilterDTOs;
using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface IJobRepository : IBaseRepository<Job>
{
    //contracttypes, addresses
    Task<IList<Job>> GetNonPrivilegedResults(JobSortFilterDto sortFilterDto, CancellationToken cancellationToken);
    // permissions, tags, contracttypes, addresses
    Task<IList<Job>> GetManageModeJobsWithNonTagPermissions(long userId, JobSortFilterDto sortFilterDto, CancellationToken cancellationToken);
    Task<IList<Job>> GetManageModeJobsWithTagPermissions(long userId, JobSortFilterDto sortFilterDto, CancellationToken cancellationToken);
    
    void AttachContractType(long jobId, long contractTypeId);
    void DetachContractType(long jobId, long contractTypeId);

    void AttachTag(long jobId, long tagId);
    void DetachTag(long jobId, long tagId);
    
    Task<UserJobPermissionSet?> GetUserPermissionSet(long jobId, long userId, CancellationToken cancellationToken);
    // Task<IList<UserJobPermissionSet>> GetAllUserPermissionSets(long jobId, CancellationToken cancellationToken);
    void AddUserPermissionSet(UserJobPermissionSet permissionSet);
    void UpdateUserPermissionSet(UserJobPermissionSet permissionSet);
    void RemoveUserPermissionSet(long jobId, long userId);
}