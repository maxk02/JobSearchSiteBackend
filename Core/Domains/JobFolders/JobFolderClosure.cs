using Core.Domains._Shared.EntityInterfaces;

namespace Core.Domains.JobFolders;

public class JobFolderClosure : IClosure<JobFolder>
{
    public long AncestorId { get; set; }
    public JobFolder? Ancestor { get; set; }
    public long DescendantId { get; set; }
    public JobFolder? Descendant { get; set; }
    public int Depth { get; set; }

    public JobFolderClosure(long ancestorId, long descendantId, int depth)
    {
        AncestorId = ancestorId;
        DescendantId = descendantId;
        Depth = depth;
    }
}