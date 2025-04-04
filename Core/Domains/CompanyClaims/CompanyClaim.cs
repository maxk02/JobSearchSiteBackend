using System.Collections.Immutable;
using Core.Domains._Shared.EntityInterfaces;

namespace Core.Domains.CompanyClaims;

public class CompanyClaim : IEntityWithId, IClaimEntity
{
    public static readonly ImmutableArray<CompanyClaim> AllValues =
    [
        new CompanyClaim(1, nameof(IsOwner)),
        new CompanyClaim(2, nameof(IsAdmin)),
        new CompanyClaim(3, nameof(CanReadStats)),
        new CompanyClaim(4, nameof(CanEditProfile))
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(permission => permission.Id)];
    
    public static readonly CompanyClaim IsOwner = AllValues.First(permission => permission.Name == nameof(IsOwner));

    public static readonly CompanyClaim IsAdmin = AllValues.First(permission => permission.Name == nameof(IsAdmin));

    public static readonly CompanyClaim CanReadStats = AllValues.First(permission => permission.Name == nameof(CanReadStats));

    public static readonly CompanyClaim CanEditProfile = AllValues.First(permission => permission.Name == nameof(CanEditProfile));
    
    public long Id { get; private set; }
    public string Name { get; private set; }

    private CompanyClaim(long id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public ICollection<UserCompanyClaim>? UserCompanyClaims { get; set; }
}