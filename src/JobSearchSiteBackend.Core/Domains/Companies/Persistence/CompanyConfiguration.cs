using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Companies.Persistence;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(company => company.Id);
        
        builder
            .HasMany(company => company.JobFolders)
            .WithOne(jobFolder => jobFolder.Company)
            .HasForeignKey(jobFolder => jobFolder.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(company => company.UserCompanyClaims)
            .WithOne(userCompanyPermission => userCompanyPermission.Company)
            .HasForeignKey(userCompanyPermission => userCompanyPermission.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(company => company.Country)
            .WithMany(country => country.Companies)
            .HasForeignKey(company => company.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(company => company.CompanyAvatars)
            .WithOne(companyAvatar => companyAvatar.Company)
            .HasForeignKey(companyAvatar => companyAvatar.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasMany(company => company.CompanyBalanceTransactions)
            .WithOne(companyBalanceTransaction => companyBalanceTransaction.Company)
            .HasForeignKey(companyBalanceTransaction => companyBalanceTransaction.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(company => company.Employees)
            .WithMany(profile => profile.CompaniesWhereEmployed);
    }
}