namespace Core.Domains._Shared.Entities.Interfaces;

public interface IHierarchicalEntity<T> where T : EntityBase
{
    // public long? ParentId { get; }
    // public T? Parent { get; }
    // public ICollection<T>? Children { get; }
    
    public ICollection<Closure<T>>? Ancestors { get; }
    public ICollection<Closure<T>>? Descendants { get; }
}