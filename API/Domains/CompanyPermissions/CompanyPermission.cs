using API.Domains._Shared.EntityInterfaces;
using API.Domains.CompanyPermissions.UserCompanyCompanyPermissions;

namespace API.Domains.CompanyPermissions;

public class CompanyPermission : IEntityWithId, IPermissionEntity
{
    public static class Values
    {
        public static readonly CompanyPermission HasFullAccess =
            new CompanyPermission(1, nameof(HasFullAccess), "");
        
        public static readonly CompanyPermission CanManagePeopleAndAssignPermissions =
            new CompanyPermission(2, nameof(CanManagePeopleAndAssignPermissions), "");
        
        public static readonly CompanyPermission CanEditStats =
            new CompanyPermission(3, nameof(CanEditStats), "");
        
        public static readonly CompanyPermission CanReadStats =
            new CompanyPermission(4, nameof(CanReadStats), "");
        
        public static readonly CompanyPermission CanEditProfile =
            new CompanyPermission(5, nameof(CanEditProfile), "");
        
        public static readonly CompanyPermission CanEditNewsfeed =
            new CompanyPermission(6, nameof(CanEditNewsfeed), "");
        
        public static readonly CompanyPermission CanContactPeopleWithPublicCv =
            new CompanyPermission(7, nameof(CanContactPeopleWithPublicCv), "");
    }
    
    private CompanyPermission(long id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    public virtual ICollection<UserCompanyCompanyPermission>? UserCompanyCompanyPermissions { get; set; }
}