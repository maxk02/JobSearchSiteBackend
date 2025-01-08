using Core.Domains.CompanyClaims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class UserCompanyClaimConfiguration : IEntityTypeConfiguration<UserCompanyClaim>
{
    public void Configure(EntityTypeBuilder<UserCompanyClaim> builder)
    {
        builder.HasKey(userCompanyClaim => userCompanyClaim.Id);

        builder
            .HasOne(userCompanyClaim => userCompanyClaim.User)
            .WithMany(user => user.UserCompanyClaims)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userCompanyClaim => userCompanyClaim.Company)
            .WithMany(company => company.UserCompanyClaims)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userCompanyClaim => userCompanyClaim.CompanyPermission)
            .WithMany(companyClaim => companyClaim.UserCompanyPermissions)
            .OnDelete(DeleteBehavior.Restrict);
    }
}