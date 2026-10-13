namespace JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

public interface IEntityWithSearchSync
{
    public bool IsDeleted { get; set; }
    public Guid VersionId { get; set; }
    public Guid? VersionIdSyncedWithSearch { get; set; }
}