using Domain._Shared.Entities;
using Domain._Shared.Entities.Interfaces;
using Domain.CompanyPermissions.UserCompanyCompanyPermissions;
using Domain.Countries;
using Domain.Folders;
using Domain.Jobs;
using Domain.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Companies;

public class Company : BaseEntity, IPublicOrPrivateEntity
{
    public static CompanyValidator Validator { get; } = new();
    
    public static Result<Company> Create(string name, string? description, bool isPublic, long countryId)
    {
        var company = new Company(name, description, isPublic, countryId);

        var validationResult = Validator.Validate(company);

        return validationResult.IsValid ? company : Result<Company>.Invalid(validationResult.AsErrors());
    }
    
    private Company(string name, string? description, bool isPublic, long countryId)
    {
        CountryId = countryId;
        Name = name;
        Description = description;
        IsPublic = isPublic;
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
    
    public bool IsPublic { get; private set; }
    public Result MakePublic()
    {
        IsPublic = true;
        return Result.Success();
    }
    public Result MakePrivate()
    {
        IsPublic = false;
        return Result.Success();
    }
    
    public virtual Country? Country { get; set; }

    public virtual ICollection<Folder>? Folders { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
    
    public virtual ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
    public virtual ICollection<UserCompanyCompanyPermission>? UserCompanyCompanyPermissions { get; set; }
}