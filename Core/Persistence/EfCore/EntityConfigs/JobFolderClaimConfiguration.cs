using Core.Domains.JobFolderClaims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobFolderClaimConfiguration : IEntityTypeConfiguration<JobFolderClaim>
{
    public void Configure(EntityTypeBuilder<JobFolderClaim> builder)
    {
        builder.HasKey(jobFolderClaim => jobFolderClaim.Id);

        builder
            .HasMany(jobFolderClaim => jobFolderClaim.UserJobFolderClaims)
            .WithOne(userJobFolderClaim => userJobFolderClaim.JobFolderClaim)
            .HasForeignKey(userJobForeignClaim => userJobForeignClaim.ClaimId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}