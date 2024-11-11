using Domain.Entities.Companies;
using Domain.Entities.Jobs;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Tags;

public class Tag : BaseEntity
{
    public static TagValidator Validator { get; } = new();

    public static Result<Tag> Create(long companyId, string name, string? description)
    {
        var tag = new Tag(companyId, name, description);

        var validationResult = Validator.Validate(tag);

        return validationResult.IsValid ? Result.Success(tag) : Result.Failure<Tag>(validationResult.Errors);
    }
    
    private Tag(long companyId, string name, string? description)
    {
        CompanyId = companyId;
        Name = name;
        Description = description;
    }
    
    public long CompanyId { get; private set; }
    
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
    
    public string? Description { get; private set; }
    public Result SetDescription(string newValue)
    {
        var oldValue = Description;
        Description = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Description = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public virtual Company? Company { get; set; }
    public virtual IList<Job>? Jobs { get; set; }
    public virtual IList<UserTagPermissionSet>? UserTagPermissionSets { get; set; }
}