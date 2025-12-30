using Elasticsearch.Net;
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
        if (locations is null || locations.Count == 0)
            throw new InvalidDataException();

        // Create index if not exists
        var existsResponse = await client.Indices.ExistsAsync(IndexName);
        if (!existsResponse.Exists)
        {
            await CreateIndexAsync();
        }

        var response = await client.BulkAsync(b => b
            .Index(IndexName)
            // .Refresh(Refresh.WaitFor)
            .UpdateMany(locations, (ud, p) => ud
                .Id(p.Id)
                .Doc(p)
                .DocAsUpsert(true)));
        
        if (!response.IsValid)
            throw new ApplicationException();
    }

    public async Task CreateIndexAsync(CancellationToken cancellationToken = default)
    {
        var result = await client.Indices.CreateAsync(
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
                            .Analyzer("autocomplete_analyzer") // Index partial words
                            .SearchAnalyzer("search_analyzer") // Search using whole words
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

        if (!result.IsValid)
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
        var searchResponse = await client.SearchAsync<LocationSearchModel>(s => s
            .Index(IndexName)
            // .Source(src => src.
            //     Includes(i => i.
            //         Field(f => f.Id)
            //     )
            // )
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
                // .Source(src => src.
                //     Includes(i => i.
                //         Field(f => f.Id)
                //     )
                // )
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
        await client.Indices.RefreshAsync(IndexName);

        var searchResponse = await client.SearchAsync<LocationSearchModel>(s => s
                .Index(IndexName)
                .Query(q => q
                    .Bool(b => b
                        .Filter(f => f
                            .Term(t => t
                                .Field(model => model.CountryId)
                                .Value(countryId)
                            )
                        )
                        .Must(m => m
                            .Match(m => m
                                .Field(f => f.FullName) // single field
                                .Query(query)
                                .Fuzziness(Fuzziness.Auto)
                            )
                        )
                    )
                )
                .Size(size), 
            cancellationToken
        );

        var results = searchResponse.Hits.Select(h => h.Source).ToList();
        
        return results;
    }
}