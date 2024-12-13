using API.Domains._Shared.EntityInterfaces;
using API.Domains.Countries;
using API.Domains.Jobs;
using API.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace API.Domains.Locations;

public class Location : IEntityWithId, IHierarchicalEntity<Location>
{
    public static LocationValidator Validator { get; } = new();
    
    public static Result<Location> Create(long countryId, long? parentId, string name, string? description, string? code)
    {
        var location = new Location(countryId, parentId, name, description, code);

        var validationResult = Validator.Validate(location);

        return validationResult.IsValid ? location : Result<Location>.Invalid(validationResult.AsErrors());
    }
    
    private Location(long countryId, long? parentId, string name, string? description, string? code)
    {
        CountryId = countryId;
        ParentId = parentId;
        Name = name;
        Description = description;
        Code = code;
    }
    
    public long Id { get; private set; }
    
    public long CountryId { get; private set; }
    
    public long? ParentId { get; private set; }

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
    
    public string? Description { get; private set; }
    public Result SetDescription(string newValue)
    {
        var oldValue = Description;
        Description = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            Description = oldValue;
            return Result.Invalid(validationResult.AsErrors());
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
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    public virtual Country? Country { get; set; }
    public virtual Location? Parent { get; set; }
    public virtual ICollection<Location>? Children { get; set; }
    public virtual ICollection<UserProfile>? Users { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
}