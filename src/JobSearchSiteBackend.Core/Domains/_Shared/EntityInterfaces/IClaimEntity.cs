namespace JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

public interface IClaimEntity : IEntityWithId
{
    public string Name { get; }
}