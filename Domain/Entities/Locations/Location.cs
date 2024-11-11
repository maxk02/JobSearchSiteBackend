using Domain.Entities.Jobs;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Locations;

public class Location : BaseEntity, ITreeEntity
{
    public static LocationValidator Validator { get; } = new();
    
    public static Result<Location> Create(long? parentId, string name, string? description, string? code)
    {
        var location = new Location(parentId, name, description, code);

        var validationResult = Validator.Validate(location);

        return validationResult.IsValid ? Result.Success(location) : Result.Failure<Location>(validationResult.Errors);
    }
    
    private Location(long? parentId, string name, string? description, string? code)
    {
        ParentId = parentId;
        Name = name;
        Description = description;
        Code = code;
    }
    
    public long? ParentId { get; private set; }
    
    public int Level { get; private set; }

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
    
    public string? Code { get; private set; }
    public Result SetCode(string newValue)
    {
        var oldValue = Code;
        Code = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Code = oldValue;
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public virtual Location? Parent { get; set; }
    public virtual IList<User>? Users { get; set; }
    public virtual IList<Job>? Jobs { get; set; }
}