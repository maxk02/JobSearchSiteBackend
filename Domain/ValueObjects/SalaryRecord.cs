using Domain.Enums;

namespace Domain.ValueObjects;

public class SalaryRecord
{
    public decimal? Minimum { get; set; }
    public decimal? Maximum { get; set; }
    public string? CurrencyCode { get; set; }
    public TimeValue? TimeValue { get; set; }
}