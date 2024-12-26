using Core.Domains.JobFolderPermissions.UserJobFolderPermissions;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class UserJobFolderPermissionConfiguration : MyBaseEntityConfiguration<UserJobFolderPermission>
{
    public override void Configure(EntityTypeBuilder<UserJobFolderPermission> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(userJobFolderPermission => userJobFolderPermission.User)
            .WithMany(user => user.UserFolderFolderPermissions)
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