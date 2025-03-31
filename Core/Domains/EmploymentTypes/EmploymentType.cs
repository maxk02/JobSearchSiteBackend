using System.Collections.Immutable;
using Core.Domains.Jobs;

namespace Core.Domains.EmploymentTypes;

public class EmploymentType
{
    public static readonly ImmutableArray<EmploymentType> AllValues =
    [
        new EmploymentType(1, "Part-time"),
        new EmploymentType(2, "Full-time"),
        new EmploymentType(3, "On-site"),
        new EmploymentType(4, "Remote"),
        new EmploymentType(5, "Hybrid"),
        new EmploymentType(6, "Mobile"),
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(category => category.Id)];

    private EmploymentType(long id, string nameEng)
    {
        Id = id;
        NameEng = nameEng;
    }
    
    public long Id { get; }
    
    public string NameEng { get; private set; }
    
    public ICollection<Job>? Jobs { get; set; }
}