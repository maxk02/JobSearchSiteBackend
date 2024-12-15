using Core.Domains._Shared.Entities;
using Core.Domains.FolderPermissions.UserFolderFolderPermissions;

namespace Core.Domains.FolderPermissions;

public class FolderPermission : Permission
{
    public static class Values
    {
        public static readonly FolderPermission HasFullAccess =
            new FolderPermission(1, Guid.Parse("0edebed0-89ec-4339-835e-83c8cfdc833c"), nameof(HasFullAccess), "");
        
        public static readonly FolderPermission CanManagePeopleAndAssignPermissions =
            new FolderPermission(2, Guid.Parse("01b52e4e-600f-475d-93bf-c7a1d3d539de"), nameof(CanManagePeopleAndAssignPermissions), "");
        
        public static readonly FolderPermission CanEditStats =
            new FolderPermission(3, Guid.Parse("4ce65449-f155-4ad6-8e80-0f5db3835090"), nameof(CanEditStats), "");
        
        public static readonly FolderPermission CanReadStats =
            new FolderPermission(4, Guid.Parse("fabb1414-c5fa-41e4-8c5f-739887213828"), nameof(CanReadStats), "");
        
        public static readonly FolderPermission CanEditInfo =
            new FolderPermission(5, Guid.Parse("2aefb9f6-d143-4234-973d-5f3da1ceefa1"), nameof(CanEditInfo), "");
        
        public static readonly FolderPermission CanEditJobsAndSubfolders =
            new FolderPermission(6, Guid.Parse("b0f18109-f32e-4e5d-9496-0b8938c1da7c"), nameof(CanEditJobsAndSubfolders), "");
        
        public static readonly FolderPermission CanManageApplications =
            new FolderPermission(7, Guid.Parse("514ec372-92dc-4428-ac48-784e94a8af87"), nameof(CanManageApplications), "");
        
        public static readonly FolderPermission CanContactPeopleWithPublicCv =
            new FolderPermission(8, Guid.Parse("f7056abb-2520-49f2-abc7-57e8fadb7f77"), nameof(CanContactPeopleWithPublicCv), "");
    }
    
    private FolderPermission(long id, Guid guidIdentifier, string name, string description) : base(id, guidIdentifier, name, description)
    {

    }
    
    public ICollection<UserFolderFolderPermission>? UserFolderFolderPermissions { get; set; }
}