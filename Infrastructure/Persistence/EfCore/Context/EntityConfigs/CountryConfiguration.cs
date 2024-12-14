using Core.Countries;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class CountryConfiguration : MyBaseEntityConfiguration<Country>
{
    public override void Configure(EntityTypeBuilder<Country> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(country => country.ContractTypes)
            .WithOne(contractType => contractType.Country)
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