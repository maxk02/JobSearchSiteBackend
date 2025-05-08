using Core.Domains.Jobs.Search;
using Elasticsearch.Net;
using Nest;

namespace Infrastructure.Search.Elasticsearch;

public class ElasticJobSearchRepository(IElasticClient client) : IJobSearchRepository
{
    public string IndexName => "jobs";
    
    public async Task SeedAsync()
    {
        // Create index if not exists
        var existsResponse = await client.Indices.ExistsAsync(IndexName);
        if (!existsResponse.Exists)
        {
            await CreateIndexAsync();
        }
    }

    public async Task AddOrUpdateIfNewestAsync(JobSearchModel searchModel, byte[] rowVersion,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (searchModel.DeletionDateTimeUtc is not null)
                throw new InvalidOperationException();

            var version = rowVersion.Length >= 8 ? BitConverter.ToInt64(rowVersion, 0) : 0;
            await client.IndexAsync(searchModel, u => u
                .Id(searchModel.Id)
                .Version(version)
                .VersionType(VersionType.ExternalGte), cancellationToken);
        }
        catch (ElasticsearchClientException ex) when (ex.Response.HttpStatusCode == 409)
        {
        }
    }

    public async Task SoftDeleteAsync(JobSearchModel searchModel, byte[] rowVersion,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (searchModel.DeletionDateTimeUtc is null)
                throw new InvalidOperationException();

            var version = rowVersion.Length >= 8 ? BitConverter.ToInt64(rowVersion, 0) : 0;
            await client.IndexAsync(searchModel, u => u
                .Id(searchModel.Id)
                .Version(version)
                .VersionType(VersionType.External), cancellationToken);
        }
        catch (ElasticsearchClientException ex) when (ex.Response.HttpStatusCode == 409)
        {
        }
    }

    public async Task CreateIndexAsync(CancellationToken cancellationToken = default)
    {
        await client.Indices.CreateAsync(
            IndexName,
            index => index
                .Map<JobSearchModel>(map => map
                    .Properties(properties => properties
                        .Number(num => num
                            .Name(n => n.Id)
                            .Type(NumberType.Long)
                        )
                        .Text(t => t
                            .Name(n => n.Title)
                        )
                        .Text(t => t
                            .Name(n => n.Description)
                        )
                        .Text(t => t
                            .Name(n => n.Responsibilities)
                        )
                        .Text(t => t
                            .Name(n => n.Requirements)
                        )
                        .Text(t => t
                            .Name(n => n.NiceToHaves)
                        )
                        .Date(d => d
                            .Name(n => n.DeletionDateTimeUtc)
                            .Index(false)
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
        var searchResponse = await client.SearchAsync<JobSearchModel>(s => s
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
        var searchResponse = await client.SearchAsync<JobSearchModel>(s => s
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
    
    public async Task<ICollection<long>> SearchFromCountriesAndCategoriesAsync(ICollection<long> countryIds,
        ICollection<long> categoryIds, string query, CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<JobSearchModel>(s => s
            .Source(src => src.
                Includes(i => i.
                    Field(f => f.Id)
                )
            )
            .Query(q => q
                .Bool(b =>
                    {
                        if (countryIds.Count > 0)
                        {
                            b = b
                                .Filter(f => f
                                    .Terms(t => t
                                        .Field(f2 => f2.CountryId)
                                        .Terms(countryIds)
                                    )
                                );
                            
                        }
                        
                        if (categoryIds.Count > 0)
                        {
                            b = b
                                .Filter(f => f
                                    .Terms(t => t
                                        .Field(f2 => f2.CategoryId)
                                        .Terms(categoryIds)
                                    )
                                );
                            
                        }

                        b = b.Must(m => m
                            .MultiMatch(mm => mm
                                .Query(query)
                                .Type(TextQueryType.BestFields)
                                .Fields("*")
                            )
                        );

                        return b;
                    }
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
}