using JobSearchSiteBackend.Core.Domains._Shared.Enums;

namespace JobSearchSiteBackend.Core.Domains.Jobs;

public class JobSalaryInfo
{
    public long JobId { get; private set; }
    public long CurrencyId { get; private set; }
    public decimal? Minimum { get; set; }
    public decimal? Maximum { get; set; }
    public UnitOfTime UnitOfTime { get; set; }
    public bool? IsAfterTaxes { get; set; }

    public JobSalaryInfo(long jobId, long currencyId, decimal? minimum,
        decimal? maximum, UnitOfTime unitOfTime, bool? isAfterTaxes)
    {
        JobId = jobId;
        CurrencyId = currencyId;
        Minimum = minimum;
        Maximum = maximum;
        UnitOfTime = unitOfTime;
        IsAfterTaxes = isAfterTaxes;
    }
}