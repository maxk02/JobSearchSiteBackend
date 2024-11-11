using Shared.Results;

namespace Domain.Shared.ValueEntities;

public class SalaryRecord
{
    public static SalaryRecordValidator Validator { get; } = new();

    public static Result<SalaryRecord> Create(decimal? minimum, decimal? maximum,
        string currencyCode, string unitOfTime)
    {
        var salaryRecord = new SalaryRecord(minimum, maximum, currencyCode, unitOfTime);

        var validationResult = Validator.Validate(salaryRecord);

        return validationResult.IsValid
            ? Result.Success(salaryRecord)
            : Result.Failure<SalaryRecord>(validationResult.Errors);
    }
    
    private SalaryRecord(decimal? minimum, decimal? maximum,
        string currencyCode, string unitOfTime)
    {
        Minimum = minimum;
        Maximum = maximum;
        CurrencyCode = currencyCode;
        UnitOfTime = unitOfTime;
    }
    
    public decimal? Minimum { get; private set; }
    public decimal? Maximum { get; private set; }
    public string CurrencyCode { get; private set; }
    public string UnitOfTime { get; private set; }
}