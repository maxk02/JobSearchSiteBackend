using System.Collections.Immutable;
using Core.Domains._Shared.EntityInterfaces;

namespace Core.Domains.CompanyClaims;

public class CompanyClaim : IEntityWithId, IClaimEntity
{
    public static readonly ImmutableArray<CompanyClaim> AllValues =
    [
        new CompanyClaim(1, nameof(IsOwner)),
        new CompanyClaim(2, nameof(IsAdmin)),
        new CompanyClaim(3, nameof(CanEditStats)),
        new CompanyClaim(4, nameof(CanReadStats)),
        new CompanyClaim(5, nameof(CanEditProfile)),
        new CompanyClaim(6, nameof(CanEditNewsfeed))
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(permission => permission.Id)];
    
    public static readonly CompanyClaim IsOwner = AllValues.First(permission => permission.Name == nameof(IsOwner));

    public static readonly CompanyClaim IsAdmin = AllValues.First(permission => permission.Name == nameof(IsAdmin));

    public static readonly CompanyClaim CanEditStats = AllValues.First(permission => permission.Name == nameof(CanEditStats));

    public static readonly CompanyClaim CanReadStats = AllValues.First(permission => permission.Name == nameof(CanReadStats));

    public static readonly CompanyClaim CanEditProfile = AllValues.First(permission => permission.Name == nameof(CanEditProfile));

    public static readonly CompanyClaim CanEditNewsfeed = AllValues.First(permission => permission.Name == nameof(CanEditNewsfeed));
    
    public long Id { get; }
    public string Name { get; }

    private CompanyClaim(long id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public virtual ICollection<UserCompanyClaim>? UserCompanyPermissions { get; set; }
}