using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

namespace JobSearchSiteBackend.Core.Domains.Locations;

public class LocationRelation : IHierarchicalEntityRelation<Location>
{
    public long AncestorId { get; set; }
    public Location? Ancestor { get; set; }
    public long DescendantId { get; set; }
    public Location? Descendant { get; set; }
    public int Depth { get; set; }

    public LocationRelation(long ancestorId, long descendantId, int depth)
    {
        AncestorId = ancestorId;
        DescendantId = descendantId;
        Depth = depth;
    }
}