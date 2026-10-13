using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Countries.Persistence;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(currency => currency.Id);
        
        builder
            .HasMany(currency => currency.CountryCurrencies)
            .WithOne(countryCurrency => countryCurrency.Currency)
            .HasForeignKey(countryCurrency => countryCurrency.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(Currency.AllValues);
    }
}