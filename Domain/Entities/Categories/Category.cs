using Domain.Entities.Jobs;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Categories;

public class Category : BaseEntity, ITreeEntity
{
    public static CategoryValidator Validator { get; } = new();
    
    public static Result<Category> Create(int? parentId, string name)
    {
        var category = new Category(parentId, name);

        var validationResult = Validator.Validate(category);
        
        return validationResult.IsValid ? Result.Success(category) : Result.Failure<Category>(validationResult.Errors);
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
            return Result.Failure(validationResult.Errors);
        }

        return Result.Success();
    }
    
    public int Level { get; private set; } //todo

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
    
    
    public virtual Category? Parent { get; set; }
    public virtual IList<Job>? Jobs { get; set; }
    public virtual IList<User>? Users { get; set; }
}