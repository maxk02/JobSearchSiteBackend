using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domains.JobFolderClaims.Persistence;

public class UserJobFolderClaimConfiguration : IEntityTypeConfiguration<UserJobFolderClaim>
{
    public void Configure(EntityTypeBuilder<UserJobFolderClaim> builder)
    {
        builder.HasKey(userJobFolderClaim => userJobFolderClaim.Id);

        builder
            .HasOne(userJobFolderClaim => userJobFolderClaim.User)
            .WithMany(user => user.UserJobFolderClaims)
            .HasForeignKey(userJobFolderClaim => userJobFolderClaim.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(userJobFolderClaim => userJobFolderClaim.JobFolder)
            .WithMany(jobFolder => jobFolder.UserJobFolderClaims)
            .HasForeignKey(userJobFolderClaim => userJobFolderClaim.FolderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(userJobFolderClaim => userJobFolderClaim.JobFolderClaim)
            .WithMany(jobFolderClaim => jobFolderClaim.UserJobFolderClaims)
            .HasForeignKey(userJobFolderClaim => userJobFolderClaim.ClaimId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}