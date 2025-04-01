using Core.Domains._Shared.Enums;

namespace Core.Domains.Jobs;

public class JobSalaryInfo
{
    public long JobId { get; private set; }
    public decimal? Minimum { get; set; }
    public decimal? Maximum { get; set; }
    public string CurrencyCode { get; set; }
    public UnitOfTime UnitOfTime { get; set; }
    public bool? IsAfterTaxes { get; set; }

    public JobSalaryInfo(long jobId, decimal? minimum, decimal? maximum, string currencyCode, UnitOfTime unitOfTime, bool? isAfterTaxes)
    {
        JobId = jobId;
        Minimum = minimum;
        Maximum = maximum;
        CurrencyCode = currencyCode;
        UnitOfTime = unitOfTime;
        IsAfterTaxes = isAfterTaxes;
    }
}