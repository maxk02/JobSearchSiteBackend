using Core.Domains.JobFolderPermissions;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

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