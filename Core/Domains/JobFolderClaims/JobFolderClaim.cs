using System.Collections.Immutable;
using Core.Domains._Shared.EntityInterfaces;

namespace Core.Domains.JobFolderClaims;

public class JobFolderClaim : IEntityWithId, IClaimEntity
{
    public static readonly ImmutableArray<JobFolderClaim> AllValues =
    [
        new JobFolderClaim(1, nameof(IsOwner)),
        new JobFolderClaim(2, nameof(IsAdmin)),
        new JobFolderClaim(3, nameof(CanReadStats)),
        new JobFolderClaim(4, nameof(CanEditInfo)),
        new JobFolderClaim(5, nameof(CanEditJobs)),
        new JobFolderClaim(6, nameof(CanReadJobs)),
        new JobFolderClaim(7, nameof(CanManageApplications))
    ];

    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(permission => permission.Id)];

    public static readonly JobFolderClaim IsOwner = AllValues.First(permission => permission.Name == nameof(IsOwner));

    public static readonly JobFolderClaim IsAdmin = AllValues.First(permission => permission.Name == nameof(IsAdmin));

    public static readonly JobFolderClaim CanReadStats = AllValues.First(permission => permission.Name == nameof(CanReadStats));

    public static readonly JobFolderClaim CanEditInfo = AllValues.First(permission => permission.Name == nameof(CanEditInfo));
    
    public static readonly JobFolderClaim CanReadJobs = AllValues.First(permission => permission.Name == nameof(CanReadJobs));

    public static readonly JobFolderClaim CanEditJobs = AllValues.First(permission => permission.Name == nameof(CanEditJobs));

    public static readonly JobFolderClaim CanManageApplications = AllValues.First(permission => permission.Name == nameof(CanManageApplications));

    public long Id { get; private set; }
    public string Name { get; private set; }

    private JobFolderClaim(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public ICollection<UserJobFolderClaim>? UserJobFolderClaims { get; set; }
}