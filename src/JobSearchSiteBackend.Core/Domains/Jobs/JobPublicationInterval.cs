using System.Collections.Immutable;
using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Countries;

namespace JobSearchSiteBackend.Core.Domains.Jobs;

public class JobPublicationInterval : IEntityWithId
{
    public static readonly ImmutableArray<JobPublicationInterval> AllValues = [
        new JobPublicationInterval(1, 1, 5, new decimal(35)),
        new JobPublicationInterval(2, 1, 30, new decimal(60)),
        new JobPublicationInterval(3, 1, 60, new decimal(120)),
        new JobPublicationInterval(4, 1, 90, new decimal(150))
    ];
    
    public static readonly ImmutableArray<long> AllIds = 
        [..AllValues.Select(jobPublicationInterval => jobPublicationInterval.Id)];
    
    // public static bool ExistsWithId(int id) => AllIds.Contains(id);
    //
    // public static JobPublicationInterval? GetById(int id) => AllValues.SingleOrDefault(v => v.Id == id);
    //
    // public static JobPublicationInterval? GetByDateTimeInterval(DateTime fromDateTimeUtc, DateTime toDateTimeUtc)
    // {
    //     var diff = toDateTimeUtc.Subtract(fromDateTimeUtc).TotalDays;
    //     
    //     var daysCeiled = (int)Math.Ceiling(diff);
    //
    //     var suitableOption = AllValues
    //         .OrderBy(v => v.MaxDaysOfPublication)
    //         .FirstOrDefault(v => v.MaxDaysOfPublication >= daysCeiled);
    //     
    //     return suitableOption;
    // }
    
    private JobPublicationInterval(long id, long countryCurrencyId,
        int maxDaysOfPublication, decimal price)
    {
        Id = id;
        CountryCurrencyId = countryCurrencyId;
        MaxDaysOfPublication = maxDaysOfPublication;
        Price = price;
    }
    
    public long Id { get; }
    
    public long CountryCurrencyId { get; private set; }
    
    public CountryCurrency? CountryCurrency { get; private set; }
    
    public int MaxDaysOfPublication { get; }
    
    public decimal Price { get; }
}