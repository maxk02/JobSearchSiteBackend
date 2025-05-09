using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.Persistence;

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

        builder.HasData(JobFolderClaim.AllValues);
    }
}