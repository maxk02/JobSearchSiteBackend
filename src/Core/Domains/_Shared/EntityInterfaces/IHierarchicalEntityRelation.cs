namespace Core.Domains._Shared.EntityInterfaces;

public interface IHierarchicalEntityRelation<T> where T : IEntityWithId
{
    public long AncestorId { get; }
    public T? Ancestor { get; }

    public long DescendantId { get; }
    public T? Descendant { get; }

    public int Depth { get; }
}