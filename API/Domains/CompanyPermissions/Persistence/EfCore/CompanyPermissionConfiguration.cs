using API.Domains.CompanyPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class CompanyPermissionConfiguration : IEntityTypeConfiguration<CompanyPermission>
{
    public void Configure(EntityTypeBuilder<CompanyPermission> builder)
    {
        builder
            .HasMany(companyPermission => companyPermission.UserCompanyCompanyPermissions)
            .WithOne(userCompanyCompanyPermission => userCompanyCompanyPermission.CompanyPermission)
            .OnDelete(DeleteBehavior.Restrict);
    }
}