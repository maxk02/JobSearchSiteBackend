using Domain.Entities.UserCompanyCompanyPermissions;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class UserCompanyCompanyPermissionConfiguration : MyBaseEntityConfiguration<UserCompanyCompanyPermission>
{
    public override void Configure(EntityTypeBuilder<UserCompanyCompanyPermission> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(userCompanyCompanyPermission => userCompanyCompanyPermission.User)
            .WithMany(user => user.UserCompanyCompanyPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userCompanyCompanyPermission => userCompanyCompanyPermission.Company)
            .WithMany(company => company.UserCompanyCompanyPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(userCompanyCompanyPermission => userCompanyCompanyPermission.CompanyPermission)
            .WithMany(companyPermission => companyPermission.UserCompanyCompanyPermissions)
            .OnDelete(DeleteBehavior.Restrict);
    }
}