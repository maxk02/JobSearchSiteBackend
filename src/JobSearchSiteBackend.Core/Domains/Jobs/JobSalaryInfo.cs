﻿using JobSearchSiteBackend.Core.Domains._Shared.Enums;

namespace JobSearchSiteBackend.Core.Domains.Jobs;

public class JobSalaryInfo
{
    public long JobId { get; private set; }
    public decimal? Minimum { get; set; }
    public decimal? Maximum { get; set; }
    public Currency Currency { get; set; }
    public UnitOfTime UnitOfTime { get; set; }
    public bool? IsAfterTaxes { get; set; }

    public JobSalaryInfo(long jobId, decimal? minimum, decimal? maximum,
        Currency currency, UnitOfTime unitOfTime, bool? isAfterTaxes)
    {
        JobId = jobId;
        Minimum = minimum;
        Maximum = maximum;
        Currency = currency;
        UnitOfTime = unitOfTime;
        IsAfterTaxes = isAfterTaxes;
    }
}