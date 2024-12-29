using Core.Domains.Locations;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class LocationConfiguration : EntityConfigurationBase<Location>
{
    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(location => location.Country)
            .WithMany(country => country.Locations);

        builder
            .OwnsMany(location => location.Subdivisions, ownedNavigationBuilder => ownedNavigationBuilder.ToJson());

        builder
            .HasMany(location => location.Jobs)
            .WithMany(job => job.Locations);
    }
}