using Core.Domains.JobFolderClaims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobFolderPermissionConfiguration : IEntityTypeConfiguration<JobFolderClaim>
{
    public void Configure(EntityTypeBuilder<JobFolderClaim> builder)
    {
        builder.HasKey(jobFolderPermission => jobFolderPermission.Id);

        builder
            .HasMany(jobFolderPermission => jobFolderPermission.UserJobFolderPermissions)
            .WithOne(userJobFolderPermission => userJobFolderPermission.JobFolderPermission)
            .OnDelete(DeleteBehavior.Restrict);
    }
}