using API.Domains.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
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