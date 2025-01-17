using Core.Domains.Cvs.Search;
using Core.Domains.Cvs.ValueEntities;
using Core.Domains.PersonalFiles.Search;
using Elasticsearch.Net;
using Nest;

namespace Infrastructure.Search.Elasticsearch;

public class ElasticPersonalFileSearchRepository(IElasticClient client) : IPersonalFileSearchRepository
{
    public string IndexName => "personalFiles";

    public async Task AddOrUpdateIfNewestAsync(PersonalFileSearchModel searchModel, byte[] rowVersion,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (searchModel.DeletionDateTimeUtc is not null)
                throw new InvalidOperationException();

            var version = BitConverter.ToInt64(rowVersion, 0);
            await client.IndexAsync(searchModel, u => u
                .Id(searchModel.Id)
                .Version(version)
                .VersionType(VersionType.ExternalGte), cancellationToken);
        }
        catch (ElasticsearchClientException ex) when (ex.Response.HttpStatusCode == 409)
        {
        }
    }

    public async Task SoftDeleteAsync(PersonalFileSearchModel searchModel, byte[] rowVersion,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (searchModel.DeletionDateTimeUtc is null)
                throw new InvalidOperationException();

            var version = BitConverter.ToInt64(rowVersion, 0);
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
                .Map<PersonalFileSearchModel>(map => map
                    .Properties(properties => properties
                        .Number(num => num
                            .Name(n => n.Id)
                            .Type(NumberType.Long)
                        )
                        .Text(t => t
                            .Name(n => n.TextContent)
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
                            .Field(doc => doc.TextContent)
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
                    .Field(doc => doc.TextContent)
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