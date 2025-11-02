using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Companies.Persistence;

public class CompanyAvatarConfiguration : IEntityTypeConfiguration<CompanyAvatar>
{
    public void Configure(EntityTypeBuilder<CompanyAvatar> builder)
    {
        builder.HasKey(companyAvatar => companyAvatar.Id);
        
        builder.HasIndex(companyAvatar => companyAvatar.GuidIdentifier).IsUnique();

        builder
            .HasOne(companyAvatar => companyAvatar.Company)
            .WithMany(company => company.CompanyAvatars)
            .HasForeignKey(companyAvatar => companyAvatar.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}