using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Infrastructure.Persistence.EfCore;

public class MainDataContextSeed
{
    public static async Task SeedLocationsAsync(MainDataContext context)
    {
        var locationsExist = await context.Locations.AnyAsync();

        if (locationsExist)
            return;
        
        var locations = await SeedFileHelper.LoadJsonAsync<List<Location>>("Domains/Locations/Seed/locations.json");

        if (locations is null || locations.Count == 0)
            throw new InvalidDataException();
        
        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();
    }
    
    public static async Task SeedLocationRelationsAsync(MainDataContext context)
    {
        var locationRelationsExist = await context.LocationRelations.AnyAsync();
        
        if (locationRelationsExist)
            return;
        
        var relations = await SeedFileHelper.LoadJsonAsync<List<LocationRelation>>("Domains/Locations/Seed/location_relations.json");
        if (relations is null || relations.Count == 0)
            throw new InvalidDataException();
        
        context.LocationRelations.AddRange(relations);
        await context.SaveChangesAsync();
    }
}