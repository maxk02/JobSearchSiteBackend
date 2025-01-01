using Core.Domains.CompanyPermissions.UserCompanyPermissions;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class UserCompanyPermissionConfiguration : EntityConfigurationBase<UserCompanyPermission>
{
    public override void Configure(EntityTypeBuilder<UserCompanyPermission> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(userCompanyCompanyPermission => userCompanyCompanyPermission.User)
            .WithMany(user => user.UserCompanyPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userCompanyCompanyPermission => userCompanyCompanyPermission.Company)
            .WithMany(company => company.UserCompanyPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userCompanyCompanyPermission => userCompanyCompanyPermission.CompanyPermission)
            .WithMany(companyPermission => companyPermission.UserCompanyPermissions)
            .OnDelete(DeleteBehavior.Restrict);
    }
}