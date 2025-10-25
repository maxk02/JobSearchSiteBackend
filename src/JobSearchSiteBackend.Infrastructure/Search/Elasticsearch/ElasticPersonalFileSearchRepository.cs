using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using Elasticsearch.Net;
using Nest;

namespace JobSearchSiteBackend.Infrastructure.Search.Elasticsearch;

public class ElasticPersonalFileSearchRepository(IElasticClient client) : IPersonalFileSearchRepository
{
    public string IndexName => "personalFiles";
    
    public async Task SeedAsync()
    {
        // Create index if not exists
        var existsResponse = await client.Indices.ExistsAsync(IndexName);
        if (!existsResponse.Exists)
        {
            await CreateIndexAsync();
        }
    }

    public async Task UpsertMultipleAsync(ICollection<PersonalFileSearchModel> searchModels,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await client.BulkAsync(b => b
                    .Index(IndexName)
                    .UpdateMany(searchModels, (ud, p) => ud
                        .Doc(p)
                        .DocAsUpsert(true)),
                cancellationToken);
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
                .Map<PersonalFileSearchModel>(map => map
                    .Properties(properties => properties
                        .Number(num => num
                            .Name(n => n.Id)
                            .Type(NumberType.Long)
                        )
                        .Text(t => t
                            .Name(n => n.Text)
                        )
                        .Date(d => d
                            .Name(n => n.DateTimeUpdatedUtc)
                            .Index(false)
                        )
                        .Boolean(b => b
                            .Name(n => n.IsDeleted)
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
        var searchResponse = await client.SearchAsync<PersonalFileSearchModel>(s => s
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
                        .Match(mt => mt
                            .Query(query)
                            .Field(doc => doc.Text)
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
        var searchResponse = await client.SearchAsync<PersonalFileSearchModel>(s => s
            .Source(src => src.
                Includes(i => i.
                    Field(doc => doc.Id)
                )
            )
            .Query(q => q
                .Match(mt => mt
                    .Query(query)
                    .Field(doc => doc.Text)
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