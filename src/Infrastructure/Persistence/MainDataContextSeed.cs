using Core.Domains.Locations;
using Core.Persistence;

namespace JobSearchSiteBackend.Infrastructure.Persistence;

public class MainDataContextSeed
{
    public static async Task SeedAsync(MainDataContext context)
    {
        if (!context.Locations.Any())
        {
            var locations = await SeedFileHelper.LoadJsonAsync<List<Location>>("Domains/Locations/Seed/locations.json");
            if (locations is not null)
            {
                await context.Locations.AddRangeAsync(locations);
                await context.SaveChangesAsync();
            }
        }

        if (!context.LocationRelations.Any())
        {
            var relations = await SeedFileHelper.LoadJsonAsync<List<LocationRelation>>("Domains/Locations/Seed/location_relations.json");
            if (relations is not null)
            {
                await context.LocationRelations.AddRangeAsync(relations);
                await context.SaveChangesAsync();
            }
        }
    }
}