namespace JobSearchSiteBackend.Main.Domains._Shared.EntityInterfaces;

public interface IClaimEntity : IEntityWithId
{
    public string Name { get; }
}