using API.Domains.CompanyPermissions.UserCompanyCompanyPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class UserCompanyCompanyPermissionConfiguration : IEntityTypeConfiguration<UserCompanyCompanyPermission>
{
    public void Configure(EntityTypeBuilder<UserCompanyCompanyPermission> builder)
    {
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