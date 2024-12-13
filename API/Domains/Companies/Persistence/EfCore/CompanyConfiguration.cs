using API.Domains.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder
            .HasMany(company => company.Folders)
            .WithOne(folder => folder.Company)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(company => company.UserCompanyCompanyPermissions)
            .WithOne(userCompanyCompanyPermission => userCompanyCompanyPermission.Company)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(company => company.Jobs)
            .WithOne(job => job.Company)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(company => company.Country)
            .WithMany(country => country.Companies)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(company => company.UsersWhoBookmarked)
            .WithMany(user => user.BookmarkedCompanies)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("CompanyBookmarks"));
    }
}