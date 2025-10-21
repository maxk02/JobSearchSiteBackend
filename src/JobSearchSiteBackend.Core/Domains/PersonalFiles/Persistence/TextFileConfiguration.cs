using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Persistence;

public class TextFileConfiguration : IEntityTypeConfiguration<TextFile>
{
    public void Configure(EntityTypeBuilder<TextFile> builder)
    {
        builder.ToTable("TextFiles");
    }
}