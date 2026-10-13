using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;

public class UserJobBookmarkConfiguration : IEntityTypeConfiguration<UserJobBookmark>
{
    public void Configure(EntityTypeBuilder<UserJobBookmark> builder)
    {
        builder.HasKey(ujb => ujb.Id);
        
        builder.HasIndex(ujb => new { ujb.UserId, ujb.JobId }).IsUnique();

        builder
            .HasOne(ujb => ujb.Job)
            .WithMany(j => j.UserJobBookmarks)
            .HasForeignKey(ujb => ujb.JobId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(ujb => ujb.UserProfile)
            .WithMany(u => u.UserJobBookmarks)
            .HasForeignKey(ujb => ujb.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}