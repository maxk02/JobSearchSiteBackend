using Core.Domains.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class BlacklistedJwtConfiguration : IEntityTypeConfiguration<BlacklistedJwt>
{
    public void Configure(EntityTypeBuilder<BlacklistedJwt> builder)
    {
        builder.ToTable("BlacklistedJwts");

        builder.HasKey(j => j.TokenId);
    }
}