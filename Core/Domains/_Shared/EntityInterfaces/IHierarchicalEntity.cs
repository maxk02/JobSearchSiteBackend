namespace Core.Domains._Shared.EntityInterfaces;

public interface IHierarchicalEntity<T, TClosure> where T : IEntityWithId where TClosure : IClosure<T>
{
    public ICollection<TClosure>? ClosuresWhereThisIsDescendant { get; }
    public ICollection<TClosure>? ClosuresWhereThisIsAncestor { get; }
}