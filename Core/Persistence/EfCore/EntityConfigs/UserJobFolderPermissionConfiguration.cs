using Core.Domains.JobFolderPermissions.UserJobFolderPermissions;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class UserJobFolderPermissionConfiguration : EntityConfigurationBase<UserJobFolderPermission>
{
    public override void Configure(EntityTypeBuilder<UserJobFolderPermission> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(userJobFolderPermission => userJobFolderPermission.User)
            .WithMany(user => user.UserFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userJobFolderPermission => userJobFolderPermission.JobFolder)
            .WithMany(folder => folder.UserJobFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userJobFolderPermission => userJobFolderPermission.JobFolderPermission)
            .WithMany(jobFolderPermission => jobFolderPermission.UserJobFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);
    }
}