namespace JobSearchSiteBackend.Main.Domains._Shared.EntityInterfaces;

public interface IEntityWithUploadStatus
{
    public bool IsUploadedSuccessfully { get; }
}