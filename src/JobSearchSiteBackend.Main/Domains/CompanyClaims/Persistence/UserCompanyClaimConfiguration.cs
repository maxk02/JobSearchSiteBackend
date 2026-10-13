using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.Persistence;

public class UserCompanyClaimConfiguration : IEntityTypeConfiguration<UserCompanyClaim>
{
    public void Configure(EntityTypeBuilder<UserCompanyClaim> builder)
    {
        builder.HasKey(userCompanyClaim => userCompanyClaim.Id);

        builder
            .HasOne(userCompanyClaim => userCompanyClaim.User)
            .WithMany(user => user.UserCompanyClaims)
            .HasForeignKey(userCompanyClaim => userCompanyClaim.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(userCompanyClaim => userCompanyClaim.Company)
            .WithMany(company => company.UserCompanyClaims)
            .HasForeignKey(userCompanyClaim => userCompanyClaim.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(userCompanyClaim => userCompanyClaim.CompanyClaim)
            .WithMany(companyClaim => companyClaim.UserCompanyClaims)
            .HasForeignKey(userCompanyClaim => userCompanyClaim.ClaimId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}