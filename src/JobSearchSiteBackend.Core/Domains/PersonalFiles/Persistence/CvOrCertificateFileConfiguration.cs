using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.Persistence;

public class CvOrCertificateFileConfiguration : IEntityTypeConfiguration<CvOrCertificateFile>
{
    public void Configure(EntityTypeBuilder<CvOrCertificateFile> builder)
    {
        builder.ToTable("CvOrCertificateFiles");
    }
}