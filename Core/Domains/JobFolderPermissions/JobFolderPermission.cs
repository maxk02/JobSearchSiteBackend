using System.Collections.Immutable;
using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.JobFolderPermissions.UserJobFolderPermissions;

namespace Core.Domains.JobFolderPermissions;

public class JobFolderPermission : EntityBase, IPermissionEntity
{
    public static readonly ImmutableArray<JobFolderPermission> AllValues =
    [
        new JobFolderPermission(1, nameof(IsOwner)),
        new JobFolderPermission(2, nameof(IsAdmin)),
        new JobFolderPermission(3, nameof(CanEditStats)),
        new JobFolderPermission(4, nameof(CanReadStats)),
        new JobFolderPermission(5, nameof(CanEditInfo)),
        new JobFolderPermission(6, nameof(CanEditJobsAndSubfolders)),
        new JobFolderPermission(7, nameof(CanManageApplications)),
        new JobFolderPermission(8, nameof(CanContactPeopleWithPublicCv))
    ];

    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(permission => permission.Id)];

    public static readonly JobFolderPermission IsOwner = AllValues.First(permission => permission.Name == nameof(IsOwner));

    public static readonly JobFolderPermission IsAdmin = AllValues.First(permission => permission.Name == nameof(IsAdmin));

    public static readonly JobFolderPermission CanEditStats = AllValues.First(permission => permission.Name == nameof(CanEditStats));

    public static readonly JobFolderPermission CanReadStats = AllValues.First(permission => permission.Name == nameof(CanReadStats));

    public static readonly JobFolderPermission CanEditInfo = AllValues.First(permission => permission.Name == nameof(CanEditInfo));

    public static readonly JobFolderPermission CanEditJobsAndSubfolders = AllValues.First(permission => permission.Name == nameof(CanEditJobsAndSubfolders));

    public static readonly JobFolderPermission CanManageApplications = AllValues.First(permission => permission.Name == nameof(CanManageApplications));

    public static readonly JobFolderPermission CanContactPeopleWithPublicCv = AllValues.First(permission => permission.Name == nameof(CanContactPeopleWithPublicCv));


    public string Name { get; }

    private JobFolderPermission(long id, string name) : base(id)
    {
        Name = name;
    }

    public ICollection<UserJobFolderPermission>? UserJobFolderPermissions { get; set; }
}