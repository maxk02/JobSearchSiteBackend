using Domain.Entities.Jobs;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.ContractTypes;

public class ContractType : BaseEntity
{
    public static ContractTypeValidator Validator { get; } = new();
    
    public static Result<ContractType> Create(string name)
    {
        var contractType = new ContractType(name);

        var validationResult = Validator.Validate(contractType);

        return validationResult.IsValid ? Result.Success(contractType) : Result.Failure<ContractType>(validationResult.Errors);
    }

    private ContractType(string name)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
    public Result SetName(string newValue)
    {
        var oldValue = Name;
        Name = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Name = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public virtual IList<Job>? Jobs { get; set; }
}