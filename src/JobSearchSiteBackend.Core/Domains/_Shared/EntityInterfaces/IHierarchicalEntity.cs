namespace JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

public interface IHierarchicalEntity<T, TClosure> where T : IEntityWithId where TClosure : IHierarchicalEntityRelation<T>
{
    public ICollection<TClosure>? RelationsWhereThisIsDescendant { get; }
    public ICollection<TClosure>? RelationsWhereThisIsAncestor { get; }
}