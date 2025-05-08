using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.JobApplications;
using Core.Domains.UserProfiles;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;

namespace Core.Domains.PersonalFiles;

public class PersonalFile : IEntityWithId, IEntityWithGuid, IEntityWithRowVersioning
{
    public PersonalFile(long userId, string name, string extension, long size)
    {
        UserId = userId;
        Name = name;
        Extension = extension;
        Size = size;
    }
    
    public long Id { get; private set; }

    public byte[] RowVersion { get; private set; } = [];
    
    public Guid GuidIdentifier { get; private set; } = Guid.NewGuid();
    
    public long UserId { get; private set; }
    
    public string Name { get; set; }
    
    public string Extension { get; private set; }
    
    public long Size { get; private set; }
    
    public UserProfile? User { get; set; }
    public ICollection<JobApplication>? JobApplications { get; set; }
}