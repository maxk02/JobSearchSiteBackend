using System.Collections.Immutable;

namespace Core.Domains.Jobs;

public class JobTimePeriodOption
{
    public static readonly ImmutableArray<JobTimePeriodOption> AllValues = [
        new(1, 0, 4),
        new(2, 5, 30),
        new(3, 31, 60),
        new(4, 61, 90),
    ];
    
    public long Id { get; set; }
    public int MinDays { get; set; }
    public int MaxDays { get; set; }

    public JobTimePeriodOption(long id, int minDays, int maxDays)
    {
        Id = id;
        MinDays = minDays;
        MaxDays = maxDays;
    }
}