using System.Collections.Immutable;
using Core.Domains.EmploymentOptions.Enums;
using Core.Domains.Jobs;

namespace Core.Domains.EmploymentOptions;

public class EmploymentOption
{
    public static readonly ImmutableArray<EmploymentOption> AllValues =
    [
        new EmploymentOption(1, EmploymentOptionType.EmploymentTime ,"Pełny etat"),
        new EmploymentOption(2, EmploymentOptionType.EmploymentTime, "Część etatu"),
        new EmploymentOption(3, EmploymentOptionType.Mobility, "W biurze"),
        new EmploymentOption(4, EmploymentOptionType.Mobility, "Zdalnie"),
        new EmploymentOption(5, EmploymentOptionType.Mobility, "Hybrydowo"),
        new EmploymentOption(6, EmploymentOptionType.Mobility, "Z wyjazdami"),
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(category => category.Id)];

    private EmploymentOption(long id, EmploymentOptionType employmentOptionType, string namePl)
    {
        Id = id;
        EmploymentOptionType = employmentOptionType;
        NamePl = namePl;
    }

    public long Id { get; private set; }

    public EmploymentOptionType EmploymentOptionType { get; private set; }
    
    public string NamePl { get; private set; }
    
    public ICollection<Job>? Jobs { get; set; }
}