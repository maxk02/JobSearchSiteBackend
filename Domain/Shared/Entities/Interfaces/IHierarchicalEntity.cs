namespace Domain.Shared.Entities.Interfaces;

public interface IHierarchicalEntity<T>
{
    public long? ParentId { get; }
    public T? Parent { get; }
    public ICollection<T>? Children { get; }
}