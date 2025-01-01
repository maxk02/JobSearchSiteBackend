using Core.Domains.JobFolderPermissions;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobFolderPermissionConfiguration : EntityConfigurationBase<JobFolderPermission>
{
    public override void Configure(EntityTypeBuilder<JobFolderPermission> builder)
    {
        base.Configure(builder);

        builder
            .HasMany(jobFolderPermission => jobFolderPermission.UserJobFolderPermissions)
            .WithOne(userJobFolderPermission => userJobFolderPermission.JobFolderPermission)
            .OnDelete(DeleteBehavior.Restrict);
    }
}