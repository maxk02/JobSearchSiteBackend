using Domain.Entities.Applications;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Shared.Results;

namespace Domain.Entities.FileInformations;

public class FileInformation : BaseEntity
{
    public static FileInformationValidator Validator { get; set; } = new();

    public static Result<FileInformation> Create(string name, string extension, int sizeInBytes)
    {
        var fileInfo = new FileInformation(name, extension, sizeInBytes);

        var validationResult = Validator.Validate(fileInfo);

        return validationResult.IsValid ? Result.Success(fileInfo) : Result.Failure<FileInformation>(validationResult.Errors);
    }
    
    private FileInformation(string name, string extension, int sizeInBytes)
    {
        Name = name;
        Extension = extension;
        SizeInBytes = sizeInBytes;
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
    
    public string Extension { get; private set; }
    
    public int SizeInBytes { get; private set; }
    
    // public byte[] Content { get; set; } = []; todo
    
    public virtual IList<User>? Users { get; set; }
    public virtual IList<Application>? MyApplications { get; set; }
}