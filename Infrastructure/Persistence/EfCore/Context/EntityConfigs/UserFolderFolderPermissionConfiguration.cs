using Domain.UserFolderFolderPermissions;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class UserFolderFolderPermissionConfiguration : MyBaseEntityConfiguration<UserFolderFolderPermission>
{
    public override void Configure(EntityTypeBuilder<UserFolderFolderPermission> builder)
    {
        base.Configure(builder);

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