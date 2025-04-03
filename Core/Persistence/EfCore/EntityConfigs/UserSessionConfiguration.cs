using Core.Domains.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(j => j.Token);

        builder.Property(session => session.Token).HasMaxLength(50);

        builder
            .HasOne(session => session.UserProfile)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(session => session.UserId);
    }
}