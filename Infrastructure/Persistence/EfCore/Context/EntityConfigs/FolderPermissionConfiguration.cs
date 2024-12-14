using Core.FolderPermissions;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class FolderPermissionConfiguration : MyBaseEntityConfiguration<FolderPermission>
{
    public override void Configure(EntityTypeBuilder<FolderPermission> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(folderPermission => folderPermission.UserFolderFolderPermissions)
            .WithOne(userFolderFolderPermission => userFolderFolderPermission.FolderPermission)
            .OnDelete(DeleteBehavior.Restrict);
    }
}