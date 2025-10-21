namespace JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

public interface IEntityWithUpdDelDate
{
    public DateTime DateTimeUpdatedUtc { get; }
    public bool IsDeleted { get; }
}