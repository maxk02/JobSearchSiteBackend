namespace Core.Domains._Shared.Entities;

public class Closure<T> where T : EntityBase
{
    public long AncestorId { get; private set; }
    public T? Ancestor { get; private set; }

    public long DescendantId { get; private set; }
    public T? Descendant { get; private set; }

    public int Depth { get; private set; }

    public Closure(long ancestorId, long descendantId, int depth)
    {
        AncestorId = ancestorId;
        DescendantId = descendantId;
        Depth = depth;
    }
}