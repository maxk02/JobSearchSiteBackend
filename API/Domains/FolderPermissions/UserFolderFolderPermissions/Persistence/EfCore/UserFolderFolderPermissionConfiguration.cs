using API.Domains.FolderPermissions.UserFolderFolderPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class UserFolderFolderPermissionConfiguration : IEntityTypeConfiguration<UserFolderFolderPermission>
{
    public void Configure(EntityTypeBuilder<UserFolderFolderPermission> builder)
    {
        builder
            .HasOne(userFolderFolderPermission => userFolderFolderPermission.User)
            .WithMany(user => user.UserFolderFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userFolderFolderPermission => userFolderFolderPermission.Folder)
            .WithMany(folder => folder.UserFolderFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userFolderFolderPermission => userFolderFolderPermission.FolderPermission)
            .WithMany(folderPermission => folderPermission.UserFolderFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);
    }
}