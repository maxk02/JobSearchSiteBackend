using System.Collections.Immutable;
using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.CompanyPermissions.UserCompanyPermissions;

namespace Core.Domains.CompanyPermissions;

public class CompanyPermission : BaseEntity, IPermissionEntity
{
    public static readonly ImmutableArray<CompanyPermission> AllValues =
    [
        new CompanyPermission(1, nameof(IsOwner)),
        new CompanyPermission(2, nameof(IsAdmin)),
        new CompanyPermission(3, nameof(CanEditStats)),
        new CompanyPermission(4, nameof(CanReadStats)),
        new CompanyPermission(5, nameof(CanEditProfile)),
        // new CompanyPermission(6, nameof(CanEditNewsfeed))
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(permission => permission.Id)];
    
    public static readonly CompanyPermission IsOwner = AllValues.First(permission => permission.Name == nameof(IsOwner));

    public static readonly CompanyPermission IsAdmin = AllValues.First(permission => permission.Name == nameof(IsAdmin));

    public static readonly CompanyPermission CanEditStats = AllValues.First(permission => permission.Name == nameof(CanEditStats));

    public static readonly CompanyPermission CanReadStats = AllValues.First(permission => permission.Name == nameof(CanReadStats));

    public static readonly CompanyPermission CanEditProfile = AllValues.First(permission => permission.Name == nameof(CanEditProfile));

    // public static readonly CompanyPermission CanEditNewsfeed = AllValues.First(permission => permission.Name == nameof(CanEditNewsfeed));
    
    
    public string Name { get; }

    private CompanyPermission(long id, string name) : base(id)
    {
        Name = name;
    }
    
    public virtual ICollection<UserCompanyPermission>? UserCompanyPermissions { get; set; }
}