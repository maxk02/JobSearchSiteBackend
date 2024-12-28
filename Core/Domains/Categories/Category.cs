using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.Cvs;
using Core.Domains.Jobs;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.Categories;

public class Category : EntityBase, IHierarchicalEntity<Category>
{
    public static CategoryValidator Validator { get; } = new();
    
    public static Result<Category> Create(string name)
    {
        var category = new Category(name);

        var validationResult = Validator.Validate(category);
        
        return validationResult.IsValid ? category : Result<Category>.Invalid(validationResult.AsErrors());
    }
    
    // public static Result<Category> Create(int? parentId, string name)
    // {
    //     var category = new Category(parentId, name);
    //
    //     var validationResult = Validator.Validate(category);
    //     
    //     return validationResult.IsValid ? category : Result<Category>.Invalid(validationResult.AsErrors());
    // }
    
    private Category(string name)
    {
        Name = name;
    }
    
    // private Category(int? parentId, string name)
    // {
    //     ParentId = parentId;
    //     Name = name;
    // }
    
    // public long? ParentId { get; private set; }

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
    
    
    // public virtual Category? Parent { get; set; }
    // public virtual ICollection<Category>? Children { get; set; }
    
    public ICollection<Closure<Category>>? Ancestors { get; set; }
    public ICollection<Closure<Category>>? Descendants { get; set; }
    
    public virtual ICollection<Job>? Jobs { get; set; }
    public virtual ICollection<Cv>? Cvs { get; set; }
}