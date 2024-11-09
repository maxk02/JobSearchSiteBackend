using Domain.Entities.Jobs;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.Categories;

public class Category : BaseEntity, ITreeEntity
{
    public static Result<Category> Create(int? parentId, int level, string name)
    {
        var category = new Category(parentId, level, name);
        
        return Result.Success(category);
    }
    
    private Category(int? parentId, int level, string name)
    {
        ParentId = parentId;
        Level = level;
        Name = name;
    }
    
    public int? ParentId { get; private set; }
    // private static LongIntValidator ParentIdValidator(long? parentId)
    // {
    //     var validator = new LongIntValidator(parentId, nameof(Category), nameof(ParentId))
    //         .MustBeEqualOrMoreThan(1);
    //
    //     return validator;
    // }
    public virtual Category? Parent { get; set; }
    
    public int Level { get; private set; }
    // private static LongIntValidator LevelValidator(long level)
    // {
    //     var validator = new LongIntValidator(level, nameof(Category), nameof(Level))
    //         .MustBeEqualOrMoreThan(0)
    //         .MustBeEqualOrLessThan(10);
    //
    //     return validator;
    // }

    public string Name { get; private set; }
    // private static StringValidator NameValidator(string name)
    // {
    //     var validator = new StringValidator(name, nameof(Category), nameof(Name))
    //         .ForbidWhitespaceOrEmpty()
    //         .AllowLetters()
    //         .AllowDigits()
    //         .AllowSpaces()
    //         .AllowPunctuation()
    //         .AllowSymbols()
    //         .LengthBetween(2, 30);
    //
    //     return validator;
    // }
    
    public virtual IList<Job>? Jobs { get; set; }
    public virtual IList<User>? Users { get; set; }
}