using System.Collections.Immutable;
using Core.Domains._Shared.Entities;
using Core.Domains.CompanyPermissions.UserCompanyCompanyPermissions;

namespace Core.Domains.CompanyPermissions;

public class CompanyPermission : Permission
{
    private static readonly ImmutableDictionary<string, CompanyPermission> Values = ImmutableDictionary.CreateRange(
    [
        new KeyValuePair<string, CompanyPermission>(nameof(HasFullAccess), new CompanyPermission(1, Guid.Parse("3bce9d01-3779-4e8c-8c77-9cff21ec3f14"), nameof(HasFullAccess))),
        new KeyValuePair<string, CompanyPermission>(nameof(CanManagePeopleAndAssignPermissions), new CompanyPermission(2, Guid.Parse("c20de331-1e03-4d13-a06f-c4de166e4cd3"), nameof(CanManagePeopleAndAssignPermissions))),
        new KeyValuePair<string, CompanyPermission>(nameof(CanEditStats), new CompanyPermission(3, Guid.Parse("01f27cdb-38aa-4a26-90b5-fccc71dc1e86"), nameof(CanEditStats))),
        new KeyValuePair<string, CompanyPermission>(nameof(CanReadStats), new CompanyPermission(4, Guid.Parse("2752b639-dc7a-4f63-8899-3aac02e8075e"), nameof(CanReadStats))),
        new KeyValuePair<string, CompanyPermission>(nameof(CanEditProfile), new CompanyPermission(5, Guid.Parse("749ead7a-9eea-4087-98b7-a7e39229d487"), nameof(CanEditProfile))),
        new KeyValuePair<string, CompanyPermission>(nameof(CanEditNewsfeed), new CompanyPermission(6, Guid.Parse("cf7acee4-cc9f-4825-ad8f-22836013c7a6"), nameof(CanEditNewsfeed))),
        new KeyValuePair<string, CompanyPermission>(nameof(CanContactPeopleWithPublicCv), new CompanyPermission(7, Guid.Parse("4c6d71fc-6bee-4562-b3ba-88aa1bd0e9c7"), nameof(CanContactPeopleWithPublicCv)))
    ]);

    public static readonly ImmutableArray<long> Ids = [..Values.Values.Select(permission => permission.Id)];
    
    public static readonly CompanyPermission HasFullAccess = Values[nameof(HasFullAccess)];

    public static readonly CompanyPermission CanManagePeopleAndAssignPermissions = Values[nameof(CanManagePeopleAndAssignPermissions)];

    public static readonly CompanyPermission CanEditStats = Values[nameof(CanEditStats)];

    public static readonly CompanyPermission CanReadStats = Values[nameof(CanReadStats)];

    public static readonly CompanyPermission CanEditProfile = Values[nameof(CanEditProfile)];

    public static readonly CompanyPermission CanEditNewsfeed = Values[nameof(CanEditNewsfeed)];

    public static readonly CompanyPermission CanContactPeopleWithPublicCv = Values[nameof(CanContactPeopleWithPublicCv)];
    
    private CompanyPermission(long id, Guid guidIdentifier, string name)
        : base(id, guidIdentifier, name) { }
    
    public virtual ICollection<UserCompanyCompanyPermission>? UserCompanyCompanyPermissions { get; set; }
}