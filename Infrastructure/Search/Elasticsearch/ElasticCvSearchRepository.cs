// using Core.Domains.Cvs.Search;
// using Core.Domains.Cvs.ValueEntities;
// using Elasticsearch.Net;
// using Nest;
//
// namespace Infrastructure.Search.Elasticsearch;
//
// public class ElasticCvSearchRepository(IElasticClient client) : ICvSearchRepository
// {
//     public string IndexName => "cvs";
//
//     public async Task AddOrUpdateIfNewestAsync(CvSearchModel searchModel, byte[] rowVersion,
//         CancellationToken cancellationToken = default)
//     {
//         try
//         {
//             if (searchModel.DeletionDateTimeUtc is not null)
//                 throw new InvalidOperationException();
//
//             var version = BitConverter.ToInt64(rowVersion, 0);
//             await client.IndexAsync(searchModel, u => u
//                 .Id(searchModel.Id)
//                 .Version(version)
//                 .VersionType(VersionType.ExternalGte), cancellationToken);
//         }
//         catch (ElasticsearchClientException ex) when (ex.Response.HttpStatusCode == 409)
//         {
//         }
//     }
//
//     public async Task SoftDeleteAsync(CvSearchModel searchModel, byte[] rowVersion,
//         CancellationToken cancellationToken = default)
//     {
//         try
//         {
//             if (searchModel.DeletionDateTimeUtc is null)
//                 throw new InvalidOperationException();
//
//             var version = BitConverter.ToInt64(rowVersion, 0);
//             await client.IndexAsync(searchModel, u => u
//                 .Id(searchModel.Id)
//                 .Version(version)
//                 .VersionType(VersionType.External), cancellationToken);
//         }
//         catch (ElasticsearchClientException ex) when (ex.Response.HttpStatusCode == 409)
//         {
//         }
//     }
//
//     public async Task CreateIndexAsync(CancellationToken cancellationToken = default)
//     {
//         await client.Indices.CreateAsync(
//             IndexName,
//             index => index
//                 .Map<CvSearchModel>(map => map
//                     .Properties(properties => properties
//                         .Number(num => num
//                             .Name(n => n.Id)
//                             .Type(NumberType.Long)
//                         )
//                         .Text(t => t
//                             .Name(n => n.Skills)
//                         )
//                         .Nested<EducationRecord>(t => t
//                             .Name(n => n.EducationRecords)
//                             .Properties(eduProps => eduProps
//                                 .Text(t2 => t2.Name(n2 => n2.Institution))
//                                 .Text(t2 => t2.Name(n2 => n2.Location))
//                                 .Text(t2 => t2.Name(n2 => n2.Faculty))
//                                 .Text(t2 => t2.Name(n2 => n2.Speciality))
//                                 .Text(t2 => t2.Name(n2 => n2.Degree))
//                                 .Text(t2 => t2.Name(n2 => n2.Description))
//                                 .Date(d2 => d2.Name(n2 => n2.DateOfStart).Index(false))
//                                 .Date(d2 => d2.Name(n2 => n2.DateOfFinish).Index(false))
//                             )
//                         )
//                         .Nested<WorkRecord>(t => t
//                             .Name(n => n.WorkRecords)
//                             .Properties(workProps => workProps
//                                 .Text(t2 => t2.Name(n2 => n2.Position))
//                                 .Text(t2 => t2.Name(n2 => n2.Company))
//                                 .Text(t2 => t2.Name(n2 => n2.Location))
//                                 .Text(t2 => t2.Name(n2 => n2.Description))
//                                 .Text(t2 => t2.Name(n2 => n2.Responsibilities))
//                             )
//                         )
//                         .Date(d => d
//                             .Name(n => n.DeletionDateTimeUtc)
//                             .Index(false)
//                         )
//                     )
//                 ),
//             cancellationToken
//         );
//     }
//
//     public async Task<bool> CheckIndexExistenceAsync(CancellationToken cancellationToken = default)
//     {
//         var response = await client.Indices.ExistsAsync(IndexName, ct: cancellationToken);
//         return response.Exists;
//     }
//
//     public async Task<ICollection<long>> SearchFromIdsAsync(ICollection<long> ids, string query,
//         CancellationToken cancellationToken = default)
//     {
//         var searchResponse = await client.SearchAsync<CvSearchModel>(s => s
//             .Source(src => src.
//                 Includes(i => i.
//                     Field(f => f.Id)
//                 )
//             )
//             .Query(q => q
//                 .Bool(b => b
//                     .Filter(f => f
//                         .Terms(t => t
//                             .Field(doc => doc.Id)
//                             .Terms(ids)
//                         )
//                     )
//                     .Must(m => m
//                         .MultiMatch(mm => mm
//                             .Query(query)
//                             .Type(TextQueryType.BestFields)
//                             .Fields("*")
//                         )
//                     )
//                 )
//             )
//             .Sort(sort => sort
//                     .Descending(SortSpecialField.Score)
//             ), 
//             cancellationToken
//         );
//
//         var resultIds = searchResponse.Hits.Select(h => h.Source.Id).ToList();
//         
//         return resultIds;
//     }
//
//     public async Task<ICollection<long>> SearchFromAllAsync(string query, CancellationToken cancellationToken = default)
//     {
//         var searchResponse = await client.SearchAsync<CvSearchModel>(s => s
//             .Source(src => src.
//                 Includes(i => i.
//                     Field(f => f.Id)
//                 )
//             )
//             .Query(q => q
//                 .MultiMatch(mm => mm
//                     .Query(query)
//                     .Type(TextQueryType.BestFields)
//                     .Fields("*")
//                 )
//             )
//             .Sort(sort => sort
//                 .Descending(SortSpecialField.Score)
//             ), 
//             cancellationToken
//         );
//
//         var resultIds = searchResponse.Hits.Select(h => h.Source.Id).ToList();
//         
//         return resultIds;
//     }
// }