using Core.Domains._Shared.Enums;

namespace Core.Domains.Jobs;

public class JobSalaryInfo
{
    public long JobId { get; private set; }
    public decimal? Minimum { get; }
    public decimal? Maximum { get; }
    public string CurrencyCode { get; }
    public UnitOfTime UnitOfTime { get; }
    public bool? IsAfterTaxes { get; }

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