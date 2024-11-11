using Domain.Entities.Jobs;
using Domain.Entities.Tags;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Companies;

public class Company : BaseEntity, IHideableEntity
{
    public static CompanyValidator Validator { get; } = new();
    
    public static Result<Company> Create(string name, string? description, bool isHidden)
    {
        var company = new Company(name, description, isHidden);

        var validationResult = Validator.Validate(company);

        return validationResult.IsValid ? Result.Success(company) : Result.Failure<Company>(validationResult.Errors);
    }
    
    private Company(string name, string? description, bool isHidden)
    {
        Name = name;
        Description = description;
        IsHidden = isHidden;
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
    
    public bool IsHidden { get; private set; }
    public Result MakeHidden()
    {
        IsHidden = true;
        return Result.Success();
    }
    public Result MakeVisible()
    {
        IsHidden = false;
        return Result.Success();
    }

    public virtual IList<Tag>? Tags { get; set; }
    public virtual IList<Job>? Jobs { get; set; }
    
    public virtual IList<UserCompanyBookmark>? UserCompanyBookmarks { get; set; }
    public virtual IList<UserCompanyPermissionSet>? UserCompanyPermissionSets { get; set; }
}