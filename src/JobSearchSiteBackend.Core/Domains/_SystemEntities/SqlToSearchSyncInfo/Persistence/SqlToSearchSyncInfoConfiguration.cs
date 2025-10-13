using JobSearchSiteBackend.Core.Domains.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains._SystemEntities.SqlToSearchSyncInfo.Persistence;

public class SqlToSearchSyncInfoConfiguration : IEntityTypeConfiguration<SqlToSearchSyncInfo>
{
    public void Configure(EntityTypeBuilder<SqlToSearchSyncInfo> builder)
    {
        builder.HasKey(info => info.Id);

        builder
            .HasIndex(info => info.CollectionName)
            .IsUnique();

        builder.HasData(SqlToSearchSyncInfo.AllValues);
    }
}