using Core.Domains.CompanyPermissions;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class CompanyPermissionConfiguration : EntityConfigurationBase<CompanyPermission>
{
    public override void Configure(EntityTypeBuilder<CompanyPermission> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(companyPermission => companyPermission.UserCompanyPermissions)
            .WithOne(userCompanyCompanyPermission => userCompanyCompanyPermission.CompanyPermission)
            .OnDelete(DeleteBehavior.Restrict);
    }
}