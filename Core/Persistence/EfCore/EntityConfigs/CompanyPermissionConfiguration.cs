using Core.Domains.CompanyClaims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class CompanyPermissionConfiguration : IEntityTypeConfiguration<CompanyClaim>
{
    public void Configure(EntityTypeBuilder<CompanyClaim> builder)
    {
        builder.HasKey(cp => cp.Id);
        
        builder
            .HasMany(companyPermission => companyPermission.UserCompanyPermissions)
            .WithOne(userCompanyCompanyPermission => userCompanyCompanyPermission.CompanyClaim)
            .OnDelete(DeleteBehavior.Restrict);
    }
}