using Core.Domains._Shared.Entities;
using Core.Domains.Countries;
using Core.Domains.Jobs;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.JobContractTypes;

public class JobContractType : EntityBase
{
    public static JobContractTypeValidator Validator { get; } = new();
    
    public static Result<JobContractType> Create(long countryId, string name)
    {
        var contractType = new JobContractType(countryId, name);

        var validationResult = Validator.Validate(contractType);

        return validationResult.IsValid ? contractType : Result<JobContractType>.Invalid(validationResult.AsErrors());
    }

    private JobContractType(long countryId, string name)
    {
        CountryId = countryId;
        Name = name;
    }
    
    public long CountryId { get; private set; }
    
    public string Name { get; private set; }
    public Result SetName(string newValue)
    {
        var oldValue = Name;
        Name = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Name = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    public virtual Country? Country { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
}