using Domain.Entities.Countries;
using Domain.Entities.Jobs;
using Domain.Shared.Entities;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Entities.ContractTypes;

public class ContractType : BaseEntity
{
    public static ContractTypeValidator Validator { get; } = new();
    
    public static Result<ContractType> Create(long countryId, string name)
    {
        var contractType = new ContractType(countryId, name);

        var validationResult = Validator.Validate(contractType);

        return validationResult.IsValid ? contractType : Result<ContractType>.Invalid(validationResult.AsErrors());
    }

    private ContractType(long countryId, string name)
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