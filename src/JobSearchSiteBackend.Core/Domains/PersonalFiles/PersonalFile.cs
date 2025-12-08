using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.JobApplications;
using JobSearchSiteBackend.Core.Domains.UserProfiles;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles;

public class PersonalFile : IEntityWithId, IEntityWithGuid, IEntityWithSearchSync, IEntityWithUploadStatus
{
    public PersonalFile(long userId, string name, string extension, long size, string text)
    {
        UserId = userId;
        Name = name;
        Extension = extension;
        Size = size;
        Text = text;
    }
    
    public long Id { get; private set; }
    
    public Guid GuidIdentifier { get; private set; } = Guid.NewGuid();
    
    public DateTime DateTimeUpdatedUtc { get; set; }
    
    public DateTime? DateTimeSyncedWithSearchUtc { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public long UserId { get; private set; }
    
    public string Name { get; set; }
    
    public string Extension { get; private set; }
    
    public long Size { get; private set; }
    
    public bool IsUploadedSuccessfully { get; set; }
    
    public string Text { get; set; }
    
    public UserProfile? User { get; set; }
    public ICollection<JobApplication>? JobApplications { get; set; }
}