using Core.Domains.Countries;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class CountryConfiguration : EntityConfigurationBase<Country>
{
    public override void Configure(EntityTypeBuilder<Country> builder)
    {
        base.Configure(builder);
        
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