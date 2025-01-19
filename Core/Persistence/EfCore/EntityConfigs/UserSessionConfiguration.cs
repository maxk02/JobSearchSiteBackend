using Core.Domains.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(j => j.TokenId);

        builder.Property(session => session.TokenId).HasMaxLength(50);
        builder.Property(session => session.LastDevice).HasMaxLength(50);
        builder.Property(session => session.LastOs).HasMaxLength(50);
        builder.Property(session => session.LastClient).HasMaxLength(50);

        builder
            .HasOne(session => session.UserProfile)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(session => session.UserId);
    }
}