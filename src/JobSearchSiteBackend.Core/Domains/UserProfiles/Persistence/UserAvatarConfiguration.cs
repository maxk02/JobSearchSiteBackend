using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;

public class UserAvatarConfiguration : IEntityTypeConfiguration<UserAvatar>
{
    public void Configure(EntityTypeBuilder<UserAvatar> builder)
    {
        builder.HasKey(userAvatar => userAvatar.Id);
        
        builder.HasIndex(userAvatar => userAvatar.GuidIdentifier).IsUnique();

        builder
            .HasOne(userAvatar => userAvatar.UserProfile)
            .WithMany(userProfile => userProfile.UserAvatars)
            .HasForeignKey(userAvatar => userAvatar.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}