using Core.Domains.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(location => location.Id);

        builder
            .HasOne(location => location.Country)
            .WithMany(country => country.Locations)
            .HasForeignKey(location => location.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .OwnsMany(location => location.Subdivisions, ownedNavigationBuilder => ownedNavigationBuilder.ToJson());

        builder
            .HasMany(location => location.Jobs)
            .WithMany(job => job.Locations);
    }
}