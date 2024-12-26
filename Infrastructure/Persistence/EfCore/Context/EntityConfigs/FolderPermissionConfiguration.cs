using Core.Domains.JobFolderPermissions;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class FolderPermissionConfiguration : MyBaseEntityConfiguration<JobFolderPermission>
{
    public override void Configure(EntityTypeBuilder<JobFolderPermission> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(folderPermission => folderPermission.UserJobFolderPermissions)
            .WithOne(userFolderFolderPermission => userFolderFolderPermission.FolderPermission)
            .OnDelete(DeleteBehavior.Restrict);
    }
}