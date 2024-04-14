using Domain.Enums;

namespace Domain.JSONEntities;

public class SalaryRecord
{
    public decimal? Minimum { get; set; }
    public decimal? Maximum { get; set; }
    public CurrencyValue? CurrencyValue { get; set; }
    public TimeValue? TimeValue { get; set; }
}