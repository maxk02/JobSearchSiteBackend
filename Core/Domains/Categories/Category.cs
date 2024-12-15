using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.Cvs;
using Core.Domains.Jobs;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.Categories;

public class Category : BaseEntity, IHierarchicalEntity<Category>
{
    public static CategoryValidator Validator { get; } = new();
    
    public static Result<Category> Create(int? parentId, string name)
    {
        var category = new Category(parentId, name);

        var validationResult = Validator.Validate(category);
        
        return validationResult.IsValid ? category : Result<Category>.Invalid(validationResult.AsErrors());
    }
    
    private Category(int? parentId, string name)
    {
        ParentId = parentId;
        Name = name;
    }
    
    public long? ParentId { get; private set; }
    public Result SetParentId(long? newValue)
    {
        var oldValue = ParentId;
        ParentId = newValue;
        
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            ParentId = oldValue;
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
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
            return Result.Invalid(validationResult.AsErrors());
        }

        return Result.Success();
    }
    
    
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category>? Children { get; set; }
    public virtual ICollection<Job>? Jobs { get; set; }
    public virtual ICollection<Cv>? Cvs { get; set; }
}