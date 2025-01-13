using Core.Domains.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

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
            .HasMany(company => company.UsersWhoBookmarked)
            .WithMany(user => user.BookmarkedCompanies)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("CompanyBookmarks"));
    }
}