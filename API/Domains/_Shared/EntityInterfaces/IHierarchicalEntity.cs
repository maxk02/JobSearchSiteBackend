namespace API.Domains._Shared.EntityInterfaces;

public interface IHierarchicalEntity<T>
{
    public long? ParentId { get; }
    public T? Parent { get; }
    public ICollection<T>? Children { get; }
}