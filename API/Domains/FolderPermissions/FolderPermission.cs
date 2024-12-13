using API.Domains._Shared.EntityInterfaces;
using API.Domains.FolderPermissions.UserFolderFolderPermissions;

namespace API.Domains.FolderPermissions;

public class FolderPermission : IEntityWithId, IPermissionEntity
{
    public static class Values
    {
        public static readonly FolderPermission HasFullAccess =
            new FolderPermission(1, nameof(HasFullAccess), "");
        
        public static readonly FolderPermission CanManagePeopleAndAssignPermissions =
            new FolderPermission(2, nameof(CanManagePeopleAndAssignPermissions), "");
        
        public static readonly FolderPermission CanEditStats =
            new FolderPermission(3, nameof(CanEditStats), "");
        
        public static readonly FolderPermission CanReadStats =
            new FolderPermission(4, nameof(CanReadStats), "");
        
        public static readonly FolderPermission CanEditInfo =
            new FolderPermission(5, nameof(CanEditInfo), "");
        
        public static readonly FolderPermission CanEditJobsAndSubfolders =
            new FolderPermission(6, nameof(CanEditJobsAndSubfolders), "");
        
        public static readonly FolderPermission CanManageApplications =
            new FolderPermission(7, nameof(CanManageApplications), "");
        
        public static readonly FolderPermission CanContactPeopleWithPublicCv =
            new FolderPermission(8, nameof(CanContactPeopleWithPublicCv), "");
    }
    
    public long Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    private FolderPermission(long id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    public ICollection<UserFolderFolderPermission>? UserFolderFolderPermissions { get; set; }
}