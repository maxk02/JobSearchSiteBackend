using Elasticsearch.Net;
using JobSearchSiteBackend.Core.Domains.JobApplications.Search;
using Nest;

namespace JobSearchSiteBackend.Infrastructure.Search.Elasticsearch;

public class ElasticJobApplicationSearchRepository(IElasticClient client) : IJobApplicationSearchRepository
{
    public string IndexName => "job_applications";
    
    public async Task SeedAsync()
    {
        // Create index if not exists
        var existsResponse = await client.Indices.ExistsAsync(IndexName);

        if (existsResponse.Exists)
        {
            var deleteResponse = await client.Indices.DeleteAsync(IndexName);

            if (!deleteResponse.IsValid)
            {
                throw new Exception($"Failed to delete index: {deleteResponse.DebugInformation}");
            }
        }

        await CreateIndexAsync();
    }

    public async Task UpsertMultipleAsync(ICollection<JobApplicationSearchModel> searchModels,
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
        var createResponse = await client.Indices.CreateAsync(
            IndexName,
            index => index
                .Settings(s => s
                    .Analysis(a => a
                        .Tokenizers(t => t
                            .EdgeNGram("autocomplete_tokenizer", e => e
                                .MinGram(2)
                                .MaxGram(20)
                                .TokenChars(TokenChar.Letter, TokenChar.Digit)
                            )
                        )
                        .Analyzers(an => an
                            .Custom("autocomplete_analyzer", ca => ca
                                .Tokenizer("autocomplete_tokenizer")
                                .Filters("lowercase") // Ensure case-insensitivity
                            )
                            .Custom("search_analyzer", ca => ca
                                .Tokenizer("standard")
                                .Filters("lowercase")
                            )
                        )
                    )
                )
                .Map<JobApplicationSearchModel>(map => map
                    .Properties(properties => properties
                        .Number(num => num
                            .Name(n => n.Id)
                            .Type(NumberType.Long)
                        )
                        .Number(num => num
                            .Name(n => n.JobId)
                            .Type(NumberType.Long)
                        )
                        .Text(t => t
                            .Name(n => n.FileTexts)
                            .Analyzer("autocomplete_analyzer") // Index partial words
                            .SearchAnalyzer("search_analyzer") // Search using whole words
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

        if (!createResponse.IsValid)
            throw new ApplicationException();
    }

    public async Task<bool> CheckIndexExistenceAsync(CancellationToken cancellationToken = default)
    {
        var response = await client.Indices.ExistsAsync(IndexName, ct: cancellationToken);
        return response.Exists;
    }

    public async Task<ICollection<long>> SearchFromIdsAsync(ICollection<long> ids, string query,
        CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<JobApplicationSearchModel>(s => s
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
                        .Match(mt => mt
                            .Query(query)
                            .Field(doc => doc.FileTexts)
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
        var searchResponse = await client.SearchAsync<JobApplicationSearchModel>(s => s
            .Index(IndexName)
            .Source(src => src.
                Includes(i => i.
                    Field(doc => doc.Id)
                )
            )
            .Query(q => q
                .Match(mt => mt
                    .Query(query)
                    .Field(doc => doc.FileTexts)
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
    
    public async Task<ICollection<long>> SearchFromJobAsync(long jobId, string query, CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<JobApplicationSearchModel>(s => s
                .Index(IndexName)
                .Source(src => src.
                    Includes(i => i.
                        Field(f => f.Id)
                    )
                )
                .Query(q => q
                    .Bool(b => b
                        .Filter(f => f
                            .Term(t => t
                                .Field(doc => doc.Id)
                                .Value(jobId)
                            )
                        )
                        .Must(m => m
                            .Match(mt => mt
                                .Query(query)
                                .Field(doc => doc.FileTexts)
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
}