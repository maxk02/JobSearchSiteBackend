using Core.Domains.CompanyPermissions;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class CompanyPermissionConfiguration : MyBaseEntityConfiguration<CompanyPermission>
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