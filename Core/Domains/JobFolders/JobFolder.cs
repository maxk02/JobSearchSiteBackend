using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.Companies;
using Core.Domains.JobFolderPermissions.UserJobFolderPermissions;
using Core.Domains.Jobs;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.JobFolders;

public class JobFolder : BaseEntity, IHierarchicalEntity<JobFolder>
{
    public static JobFolderValidator Validator { get; } = new();

    public static Result<JobFolder> Create(long? companyId, long? parentId, string? name, string? description)
    {
        var folder = new JobFolder(companyId, parentId, name, description);

        var validationResult = Validator.Validate(folder);

        return validationResult.IsValid ? folder : Result<JobFolder>.Invalid();
    }
    
    private JobFolder(long? companyId, long? parentId, string? name, string? description)
    {
        CompanyId = companyId;
        ParentId = parentId;
        Name = name;
        Description = description;
    }
    
    public long? CompanyId { get; private set; }
    
    public long? ParentId { get; private set; }

    public string? Name { get; private set; }

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
            Name = Description;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    
    public virtual Company? Company { get; set; }
    public virtual JobFolder? Parent { get; set; }
    public virtual ICollection<JobFolder>? Children { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
    public virtual ICollection<UserJobFolderPermission>? UserJobFolderPermissions { get; set; }
}