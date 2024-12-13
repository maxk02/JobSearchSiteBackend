using API.Domains.FolderPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class FolderPermissionConfiguration : IEntityTypeConfiguration<FolderPermission>
{
    public void Configure(EntityTypeBuilder<FolderPermission> builder)
    {
        builder
            .HasMany(folderPermission => folderPermission.UserFolderFolderPermissions)
            .WithOne(userFolderFolderPermission => userFolderFolderPermission.FolderPermission)
            .OnDelete(DeleteBehavior.Restrict);
    }
}