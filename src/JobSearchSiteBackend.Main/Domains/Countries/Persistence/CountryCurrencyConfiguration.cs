using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Countries.Persistence;

public class CountryCurrencyConfiguration : IEntityTypeConfiguration<CountryCurrency>
{
    public void Configure(EntityTypeBuilder<CountryCurrency> builder)
    {
        builder.HasKey(currency => currency.Id);
        
        builder
            .HasOne(countryCurrency => countryCurrency.Country)
            .WithMany(country => country.CountryCurrencies)
            .HasForeignKey(countryCurrency => countryCurrency.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(countryCurrency => countryCurrency.Currency)
            .WithMany(currency => currency.CountryCurrencies)
            .HasForeignKey(countryCurrency => countryCurrency.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(CountryCurrency.AllValues);
    }
}