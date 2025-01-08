using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.JobApplications;
using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.PersonalFiles;

public class PersonalFile : IEntityWithId, IEntityWithGuid
{
    public PersonalFile(long userId, string name, string extension, long size)
    {
        UserId = userId;
        Name = name;
        Extension = extension;
        Size = size;
    }
    
    public long Id { get; set; }
    
    public Guid GuidIdentifier { get; } = Guid.NewGuid();
    
    public long UserId { get; private set; }
    
    public string Name { get; set; }
    
    public string Extension { get; private set; }
    
    public long Size { get; private set; }
    
    public virtual UserProfile? User { get; set; }
    public virtual ICollection<JobApplication>? JobApplications { get; set; }
}