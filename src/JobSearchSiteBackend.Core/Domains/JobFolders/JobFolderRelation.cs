using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

namespace JobSearchSiteBackend.Core.Domains.JobFolders;

public class JobFolderRelation : IHierarchicalEntityRelation<JobFolder>
{
    public long AncestorId { get; set; }
    public JobFolder? Ancestor { get; set; }
    public long DescendantId { get; set; }
    public JobFolder? Descendant { get; set; }
    public int Depth { get; set; }

    public JobFolderRelation(long ancestorId, long descendantId, int depth)
    {
        AncestorId = ancestorId;
        DescendantId = descendantId;
        Depth = depth;
    }
}