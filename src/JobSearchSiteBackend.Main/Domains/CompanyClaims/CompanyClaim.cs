using System.Collections.Immutable;
using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims;

public class CompanyClaim : IEntityWithId, IClaimEntity
{
    public static readonly ImmutableArray<CompanyClaim> AllValues =
    [
        new CompanyClaim(1, nameof(IsOwner)),
        new CompanyClaim(2, nameof(IsAdmin)),
        new CompanyClaim(3, nameof(CanReadStats)),
        new CompanyClaim(4, nameof(CanEditProfile)),
        new CompanyClaim(5, nameof(CanManageBalance)),
        new CompanyClaim(6, nameof(CanEditJobs)),
        new CompanyClaim(7, nameof(CanReadJobs)),
        new CompanyClaim(8, nameof(CanManageApplications))
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(permission => permission.Id)];
    
    public static readonly CompanyClaim IsOwner = AllValues.First(permission => permission.Name == nameof(IsOwner));

    public static readonly CompanyClaim IsAdmin = AllValues.First(permission => permission.Name == nameof(IsAdmin));

    public static readonly CompanyClaim CanReadStats = AllValues.First(permission => permission.Name == nameof(CanReadStats));

    public static readonly CompanyClaim CanEditProfile = AllValues.First(permission => permission.Name == nameof(CanEditProfile));
    
    public static readonly CompanyClaim CanManageBalance = AllValues.First(permission => permission.Name == nameof(CanManageBalance));

    public static readonly CompanyClaim CanReadJobs = AllValues.First(permission => permission.Name == nameof(CanReadJobs));

    public static readonly CompanyClaim CanEditJobs = AllValues.First(permission => permission.Name == nameof(CanEditJobs));

    public static readonly CompanyClaim CanManageApplications = AllValues.First(permission => permission.Name == nameof(CanManageApplications));
    
    public long Id { get; private set; }
    public string Name { get; private set; }

    private CompanyClaim(long id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
}