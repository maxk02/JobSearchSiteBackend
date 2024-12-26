using Core.Domains.JobFolderPermissions.UserJobFolderPermissions;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class UserFolderFolderPermissionConfiguration : MyBaseEntityConfiguration<UserJobFolderPermission>
{
    public override void Configure(EntityTypeBuilder<UserJobFolderPermission> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(userFolderFolderPermission => userFolderFolderPermission.User)
            .WithMany(user => user.UserFolderFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userFolderFolderPermission => userFolderFolderPermission.Folder)
            .WithMany(folder => folder.UserJobFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userFolderFolderPermission => userFolderFolderPermission.FolderPermission)
            .WithMany(folderPermission => folderPermission.UserJobFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);
    }
}