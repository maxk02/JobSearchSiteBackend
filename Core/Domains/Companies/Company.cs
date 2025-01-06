using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.CompanyPermissions.UserCompanyPermissions;
using Core.Domains.Countries;
using Core.Domains.JobFolders;
using Core.Domains.Jobs;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.Companies;

public class Company : EntityBase
{
    public static CompanyValidator Validator { get; } = new();
    
    public static Result<Company> Create(string name, string? description, bool isPublic, long countryId, long id = 0)
    {
        var company = new Company(name, description, isPublic, countryId, id);

        var validationResult = Validator.Validate(company);

        return validationResult.IsValid ? company : Result<Company>.Invalid(validationResult.AsErrors());
    }
    
    //todo id in constructor
    private Company(string name, string? description, bool isPublic, long countryId, long id = 0)
    {
        Id = id;
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
    
    public virtual Country? Country { get; set; }

    public virtual ICollection<JobFolder>? JobFolders { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
    
    public virtual ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
    public virtual ICollection<UserCompanyPermission>? UserCompanyPermissions { get; set; }
}