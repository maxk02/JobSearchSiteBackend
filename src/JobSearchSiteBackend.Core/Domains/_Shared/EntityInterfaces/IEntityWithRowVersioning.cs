namespace JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

public interface IEntityWithRowVersioning
{
    public byte[] RowVersion { get; }
}