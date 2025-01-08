using Core.Domains.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(country => country.Id);
        
        builder
            .HasMany(country => country.JobContractTypes)
            .WithOne(jobContractType => jobContractType.Country)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(country => country.Companies)
            .WithOne(company => company.Country)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(country => country.Locations)
            .WithOne(location => location.Country)
            .OnDelete(DeleteBehavior.Restrict);
    }
}