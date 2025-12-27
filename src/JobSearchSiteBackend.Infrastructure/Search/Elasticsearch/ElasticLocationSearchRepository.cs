using JobSearchSiteBackend.Core.Domains.Locations.Search;
using Nest;

namespace JobSearchSiteBackend.Infrastructure.Search.Elasticsearch;

public class ElasticLocationSearchRepository(IElasticClient client) : ILocationSearchRepository
{
    public string IndexName => "locations";
    
    public async Task SeedAsync()
    {
        var locations = await SeedFileHelper
            .LoadJsonAsync<List<LocationSearchModel>>("Domains/Locations/Seed/locations_search.json");
        if (locations is null)
            return;

        // Create index if not exists
        var existsResponse = await client.Indices.ExistsAsync(IndexName);
        if (!existsResponse.Exists)
        {
            await CreateIndexAsync();
        }

        // Bulk index locations
        var bulkRequest = new BulkRequest(IndexName)
        {
            Operations = new List<IBulkOperation>()
        };

        foreach (var location in locations)
        {
            bulkRequest.Operations.Add(new BulkIndexOperation<LocationSearchModel>(location)
            {
                Id = location.Id.ToString()
            });
        }

        var bulkResponse = await client.BulkAsync(bulkRequest);

        if (bulkResponse.Errors)
        {
            foreach (var item in bulkResponse.ItemsWithErrors)
            {
                Console.WriteLine($"Failed to index document {item.Id}: {item.Error}");
            }
        }
    }

    public async Task CreateIndexAsync(CancellationToken cancellationToken = default)
    {
        await client.Indices.CreateAsync(
            IndexName,
            index => index
                .Map<LocationSearchModel>(map => map
                    .Properties(properties => properties
                        .Number(num => num
                            .Name(n => n.Id)
                            .Type(NumberType.Long)
                        )
                        .Number(num => num
                            .Name(n => n.CountryId)
                            .Type(NumberType.Long)
                        )
                        .Text(t => t
                            .Name(n => n.FullName)
                        )
                        .Text(t => t
                            .Name(n => n.Description)
                        )
                        .Number(t => t
                            .Name(n => n.ParentIds)
                            .Type(NumberType.Long)
                        )
                        .Text(t => t
                            .Name(n => n.Code)
                        )
                    )
                ),
            cancellationToken
        );
    }

    public async Task<bool> CheckIndexExistenceAsync(CancellationToken cancellationToken = default)
    {
        var response = await client.Indices.ExistsAsync(IndexName, ct: cancellationToken);
        return response.Exists;
    }

    public async Task<ICollection<long>> SearchFromIdsAsync(ICollection<long> ids, string query,
        CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<LocationSearchModel>(s => s
            .Index(IndexName)
            .Source(src => src.
                Includes(i => i.
                    Field(f => f.Id)
                )
            )
            .Query(q => q
                .Bool(b => b
                    .Filter(f => f
                        .Terms(t => t
                            .Field(doc => doc.Id)
                            .Terms(ids)
                        )
                    )
                    .Must(m => m
                        .MultiMatch(mm => mm
                            .Query(query)
                            .Type(TextQueryType.BestFields)
                            .Fields("*")
                        )
                    )
                )
            )
            .Sort(sort => sort
                    .Descending(SortSpecialField.Score)
            ), 
            cancellationToken
        );

        var resultIds = searchResponse.Hits.Select(h => h.Source.Id).ToList();
        
        return resultIds;
    }

    public async Task<ICollection<long>> SearchFromAllAsync(string query, CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<LocationSearchModel>(s => s
                .Index(IndexName)
                .Source(src => src.
                    Includes(i => i.
                        Field(f => f.Id)
                    )
                )
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(query)
                        .Type(TextQueryType.BestFields)
                        .Fields("*")
                    )
                )
                .Sort(sort => sort
                    .Descending(SortSpecialField.Score)
                ), 
            cancellationToken
        );

        var resultIds = searchResponse.Hits.Select(h => h.Source.Id).ToList();
        
        return resultIds;
    }
    
    public async Task<ICollection<LocationSearchModel>> SearchFromCountryIdAsync(long countryId, string query,
        int size, CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<LocationSearchModel>(s => s
                .Index(IndexName)
                .Source(src => src.
                    Includes(i => i.
                        Field(f => f.Id)
                    )
                )
                .Query(q => q
                    .Bool(b => b
                        .Filter(f => f
                            .Term(t => t.CountryId, countryId)
                        )
                        .Must(m => m
                            .MultiMatch(mm => mm
                                .Query(query)
                                .Type(TextQueryType.BestFields)
                                .Fields("*")
                            )
                        )
                    )
                )
                .Sort(sort => sort
                    .Descending(SortSpecialField.Score)
                )
                .Size(size), 
            cancellationToken
        );

        var results = searchResponse.Hits.Select(h => h.Source).ToList();
        
        return results;
    }
}