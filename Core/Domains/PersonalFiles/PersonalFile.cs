using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains.JobApplications;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.PersonalFiles;

public class PersonalFile : EntityBase, IEntityWithGuid
{
    public static PersonalFileValidator Validator { get; set; } = new();

    public static Result<PersonalFile> Create(long userId, string name, string extension, long size)
    {
        var fileInfo = new PersonalFile(userId, name, extension, size);

        var validationResult = Validator.Validate(fileInfo);

        return validationResult.IsValid ? fileInfo : Result<PersonalFile>.Invalid(validationResult.AsErrors());
    }
    
    private PersonalFile(long userId, string name, string extension, long size)
    {
        UserId = userId;
        Name = name;
        Extension = extension;
        Size = size;
    }
    
    public Guid GuidIdentifier { get; } = Guid.NewGuid();
    
    public long UserId { get; private set; }
    
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
    
    public string Extension { get; private set; }
    
    public long Size { get; private set; }
    
    public virtual UserProfile? User { get; set; }
    public virtual ICollection<JobApplication>? JobApplications { get; set; }
}