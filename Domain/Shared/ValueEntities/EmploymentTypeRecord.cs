using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Shared.ValueEntities;

public class EmploymentTypeRecord
{
    public static EmploymentTypeRecordValidator Validator { get; } = new();

    public static Result<EmploymentTypeRecord> Create(bool isPartTime, bool isFullTime, bool isOnSite,
        bool isRemote, bool isHybrid, bool isMobile)
    {
        var record = new EmploymentTypeRecord(isPartTime, isFullTime, isOnSite, isRemote, isHybrid, isMobile);

        var validationResult = Validator.Validate(record);

        return validationResult.IsValid ? record : Result<EmploymentTypeRecord>.Invalid(validationResult.AsErrors());
    }
    
    private EmploymentTypeRecord(bool isPartTime, bool isFullTime, bool isOnSite,
        bool isRemote, bool isHybrid, bool isMobile)
    {
        IsPartTime = isPartTime;
        IsFullTime = isFullTime;
        IsOnSite = isOnSite;
        IsRemote = isRemote;
        IsHybrid = isHybrid;
        IsMobile = isMobile;
    }
    
    public bool IsPartTime { get; private set; }
    public bool IsFullTime { get; private set; }
    public bool IsOnSite { get; private set; }
    public bool IsRemote { get; private set; }
    public bool IsHybrid { get; private set; }
    public bool IsMobile { get; private set; }
}