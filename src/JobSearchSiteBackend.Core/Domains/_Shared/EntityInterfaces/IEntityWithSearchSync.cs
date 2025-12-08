namespace JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

public interface IEntityWithSearchSync : IEntityWithUpdDelDate
{
    public DateTime? DateTimeSyncedWithSearchUtc { get; set; }
}