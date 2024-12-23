using System.Collections.Immutable;

namespace Core.Domains.JobApplications.Values;

public static class JobApplicationStatuses
{
    public static readonly ImmutableArray<string> Values =
    [
        nameof(Submitted),
        nameof(Shortlisted),
        nameof(OfferSent),
        nameof(Rejected),
    ];
    
    public static string Submitted = Values.First(x => x == nameof(Submitted));
    public static string Shortlisted = Values.First(x => x == nameof(Shortlisted));
    public static string OfferSent = Values.First(x => x == nameof(OfferSent));
    public static string Rejected = Values.First(x => x == nameof(Rejected));
}