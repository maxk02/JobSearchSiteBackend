using Domain.Entities.Locations;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class LocationConfiguration : MyBaseEntityConfiguration<Location>
{
    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(location => location.Country)
            .WithMany(country => country.Locations);

        builder
            .HasOne(location => location.Parent)
            .WithMany(location => location.Children)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(location => location.Children)
            .WithOne(location => location.Parent)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(location => location.Jobs)
            .WithMany(job => job.Locations);
    }
}