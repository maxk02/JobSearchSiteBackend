namespace JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

public interface IEntityWithUploadStatus
{
    public bool IsUploadedSuccessfully { get; }
}