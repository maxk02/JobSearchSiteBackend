using System.Collections.Immutable;
using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.FolderPermissions.UserFolderFolderPermissions;

namespace Core.Domains.FolderPermissions;

public class FolderPermission : BaseEntity, IPermissionEntity
{
    public static readonly ImmutableArray<FolderPermission> AllValues =
    [
        new FolderPermission(1, nameof(HasFullAccess)),
        new FolderPermission(2, nameof(CanManagePeopleAndAssignPermissions)),
        new FolderPermission(3, nameof(CanEditStats)),
        new FolderPermission(4, nameof(CanReadStats)),
        new FolderPermission(5, nameof(CanEditInfo)),
        new FolderPermission(6, nameof(CanEditJobsAndSubfolders)),
        new FolderPermission(7, nameof(CanManageApplications)),
        new FolderPermission(8, nameof(CanContactPeopleWithPublicCv))
    ];

    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(permission => permission.Id)];

    public static readonly FolderPermission HasFullAccess = AllValues.First(permission => permission.Name == nameof(HasFullAccess));

    public static readonly FolderPermission CanManagePeopleAndAssignPermissions = AllValues.First(permission => permission.Name == nameof(CanManagePeopleAndAssignPermissions));

    public static readonly FolderPermission CanEditStats = AllValues.First(permission => permission.Name == nameof(CanEditStats));

    public static readonly FolderPermission CanReadStats = AllValues.First(permission => permission.Name == nameof(CanReadStats));

    public static readonly FolderPermission CanEditInfo = AllValues.First(permission => permission.Name == nameof(CanEditInfo));

    public static readonly FolderPermission CanEditJobsAndSubfolders = AllValues.First(permission => permission.Name == nameof(CanEditJobsAndSubfolders));

    public static readonly FolderPermission CanManageApplications = AllValues.First(permission => permission.Name == nameof(CanManageApplications));

    public static readonly FolderPermission CanContactPeopleWithPublicCv = AllValues.First(permission => permission.Name == nameof(CanContactPeopleWithPublicCv));


    public string Name { get; }

    private FolderPermission(long id, string name) : base(id)
    {
        Name = name;
    }

    public ICollection<UserFolderFolderPermission>? UserFolderFolderPermissions { get; set; }
}