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
    // take it from last db record sorted by date
    // next update from > from that datetime
    // need to insert date here inside db (maybe with trigger) after commit, not from asp.net
    public DateTime? UpdatedUpToDateTimeUtc { get; set; }
    // just logging from asp.net when was it synced last time
    public DateTime? LastTimeSyncedUtc { get; set; }
    
    public SqlToSearchSyncInfo(long id, string collectionName, DateTime? updatedUpToDateTimeUtc = null)
    {
        Id = id;
        CollectionName = collectionName;
        UpdatedUpToDateTimeUtc = updatedUpToDateTimeUtc;
    }
}