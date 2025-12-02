using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;

public class UserJobApplicationBookmarkConfiguration : IEntityTypeConfiguration<UserJobApplicationBookmark>
{
    public void Configure(EntityTypeBuilder<UserJobApplicationBookmark> builder)
    {
        builder.HasKey(ujab => ujab.Id);
        
        builder.HasIndex(ujab => new { ujab.UserId, ujab.JobApplicationId }).IsUnique();

        builder
            .HasOne(ujb => ujb.JobApplication)
            .WithMany(ja => ja.UserJobApplicationBookmarks)
            .HasForeignKey(ujab => ujab.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(ujb => ujb.UserProfile)
            .WithMany(u => u.UserJobApplicationBookmarks)
            .HasForeignKey(ujb => ujb.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}