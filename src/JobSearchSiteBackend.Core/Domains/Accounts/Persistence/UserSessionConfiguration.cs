﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Accounts.Persistence;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(j => j.TokenId);

        builder.Property(session => session.TokenId).HasMaxLength(50);

        builder
            .HasOne(session => session.UserProfile)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(session => session.UserId);
    }
}