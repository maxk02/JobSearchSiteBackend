using Core.Domains.JobFolderClaims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class UserJobFolderClaimConfiguration : IEntityTypeConfiguration<UserJobFolderClaim>
{
    public void Configure(EntityTypeBuilder<UserJobFolderClaim> builder)
    {
        builder.HasKey(userJobFolderClaim => userJobFolderClaim.Id);

        builder
            .HasOne(userJobFolderClaim => userJobFolderClaim.User)
            .WithMany(user => user.UserJobFolderClaims)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userJobFolderClaim => userJobFolderClaim.JobFolder)
            .WithMany(jobFolder => jobFolder.UserJobFolderClaims)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userJobFolderClaim => userJobFolderClaim.JobFolderClaim)
            .WithMany(jobFolderClaim => jobFolderClaim.UserJobFolderPermissions)
            .OnDelete(DeleteBehavior.Restrict);
    }
}