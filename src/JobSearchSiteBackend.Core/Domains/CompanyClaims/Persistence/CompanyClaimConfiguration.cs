using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.Persistence;

public class CompanyClaimConfiguration : IEntityTypeConfiguration<CompanyClaim>
{
    public void Configure(EntityTypeBuilder<CompanyClaim> builder)
    {
        builder.HasKey(companyClaim => companyClaim.Id);
        
        builder
            .HasMany(companyClaim => companyClaim.UserCompanyClaims)
            .WithOne(userCompanyClaim => userCompanyClaim.CompanyClaim)
            .HasForeignKey(userCompanyClaim => userCompanyClaim.ClaimId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}