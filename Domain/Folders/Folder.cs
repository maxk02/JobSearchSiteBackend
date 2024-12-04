using Domain._Shared.Entities;
using Domain._Shared.Entities.Interfaces;
using Domain.Companies;
using Domain.Jobs;
using Domain.UserFolderFolderPermissions;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Domain.Folders;

public class Folder : BaseEntity, IHierarchicalEntity<Folder>
{
    public static FolderValidator Validator { get; } = new();

    public Result<Folder> Create(long? companyId, long? parentId, string name, string? description)
    {
        var folder = new Folder(companyId, parentId, name, description);

        var validationResult = Validator.Validate(folder);

        return validationResult.IsValid ? folder : Result.Invalid();
    }
    
    private Folder(long? companyId, long? parentId, string name, string? description)
    {
        CompanyId = companyId;
        ParentId = parentId;
        Name = name;
        Description = description;
    }
    
    public long? CompanyId { get; private set; }
    
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
            Name = Description;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    
    public virtual Company? Company { get; set; }
    public virtual Folder? Parent { get; set; }
    public virtual ICollection<Folder>? Children { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
    public virtual ICollection<UserFolderFolderPermission>? UserFolderFolderPermissions { get; set; }
}