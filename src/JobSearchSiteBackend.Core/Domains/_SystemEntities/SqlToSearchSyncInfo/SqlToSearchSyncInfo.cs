using System.Collections.Immutable;
using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

namespace JobSearchSiteBackend.Core.Domains._SystemEntities.SqlToSearchSyncInfo;

public class SqlToSearchSyncInfo : IEntityWithId
{
    public static readonly ImmutableArray<SqlToSearchSyncInfo> AllValues = [
        new(1, "Companies"),
        new(2, "Jobs"),
        new(3, "PersonalFiles")
    ];
    
    public long Id { get; private set; }
    public string CollectionName { get; private set; }
    public DateTime? UpdatedUpToDateTimeUtc { get; private set; }
    
    public SqlToSearchSyncInfo(long id, string collectionName, DateTime? updatedUpToDateTimeUtc = null)
    {
        Id = id;
        CollectionName = collectionName;
        UpdatedUpToDateTimeUtc = updatedUpToDateTimeUtc;
    }
}