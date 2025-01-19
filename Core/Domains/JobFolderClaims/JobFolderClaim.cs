using System.Collections.Immutable;
using Core.Domains._Shared.EntityInterfaces;

namespace Core.Domains.JobFolderClaims;

public class JobFolderClaim : IEntityWithId, IClaimEntity
{
    public static readonly ImmutableArray<JobFolderClaim> AllValues =
    [
        new JobFolderClaim(1, nameof(IsOwner)),
        new JobFolderClaim(2, nameof(IsAdmin)),
        new JobFolderClaim(3, nameof(CanEditStats)),
        new JobFolderClaim(4, nameof(CanReadStats)),
        new JobFolderClaim(5, nameof(CanEditInfo)),
        new JobFolderClaim(6, nameof(CanEditJobsAndSubfolders)),
        new JobFolderClaim(7, nameof(CanReadJobsAndSubfolders)),
        new JobFolderClaim(8, nameof(CanManageApplications)),
        new JobFolderClaim(9, nameof(CanContactPeopleWithPublicCv))
    ];

    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(permission => permission.Id)];

    public static readonly JobFolderClaim IsOwner = AllValues.First(permission => permission.Name == nameof(IsOwner));

    public static readonly JobFolderClaim IsAdmin = AllValues.First(permission => permission.Name == nameof(IsAdmin));

    public static readonly JobFolderClaim CanEditStats = AllValues.First(permission => permission.Name == nameof(CanEditStats));

    public static readonly JobFolderClaim CanReadStats = AllValues.First(permission => permission.Name == nameof(CanReadStats));

    public static readonly JobFolderClaim CanEditInfo = AllValues.First(permission => permission.Name == nameof(CanEditInfo));
    
    public static readonly JobFolderClaim CanReadJobsAndSubfolders = AllValues.First(permission => permission.Name == nameof(CanReadJobsAndSubfolders));

    public static readonly JobFolderClaim CanEditJobsAndSubfolders = AllValues.First(permission => permission.Name == nameof(CanEditJobsAndSubfolders));

    public static readonly JobFolderClaim CanManageApplications = AllValues.First(permission => permission.Name == nameof(CanManageApplications));

    public static readonly JobFolderClaim CanContactPeopleWithPublicCv = AllValues.First(permission => permission.Name == nameof(CanContactPeopleWithPublicCv));

    public long Id { get; private set; }
    public string Name { get; private set; }

    private JobFolderClaim(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
}