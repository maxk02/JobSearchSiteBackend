using Core.Domains.Locations.Search;
using Nest;

namespace Infrastructure.Search.Elasticsearch;

public class ElasticLocationSearchRepository(IElasticClient client) : ILocationSearchRepository
{
    public string IndexName => "locations";

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
                            .Name(n => n.Name)
                        )
                        .Text(t => t
                            .Name(n => n.Description)
                        )
                        .Text(t => t
                            .Name(n => n.Subdivisions)
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
    
    public async Task<ICollection<long>> SearchFromCountryIdAsync(long countryId, string query,
        CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<LocationSearchModel>(s => s
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
                ), 
            cancellationToken
        );

        var resultIds = searchResponse.Hits.Select(h => h.Source.Id).ToList();
        
        return resultIds;
    }

    public async Task AddOrUpdateManyAsync(ICollection<LocationSearchModel> searchModels,
        CancellationToken cancellationToken = default)
    {
        var bulkResponse = await client
            .BulkAsync(b => b.Index(IndexName).IndexMany(searchModels), cancellationToken
        );

        if (!bulkResponse.IsValid)
            throw new InvalidDataException();
    }

    public async Task DeleteManyAsync(ICollection<long> searchModelIds, CancellationToken cancellationToken = default)
    {
        var bulkResponse = await client.BulkAsync(b => b
                .Index(IndexName)
                .DeleteMany<LocationSearchModel>(searchModelIds, (bd, id) => bd.Id(id)),
            cancellationToken
        );
        
        if (!bulkResponse.IsValid)
            throw new InvalidDataException();
    }
}