using Core.Domains._Shared.EntityInterfaces;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Categories;
using Core.Domains.Companies;
using Core.Domains.JobApplications;
using Core.Domains.JobContractTypes;
using Core.Domains.JobFolders;
using Core.Domains.Locations;
using Core.Domains.UserProfiles;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;

namespace Core.Domains.Jobs;

public class Job : IEntityWithId, IEntityWithRowVersioning
{
    public Job(long categoryId, long jobFolderId, string title, string description, bool isPublic, DateTime dateTimePublishedUtc,
        DateTime dateTimeExpiringUtc, ICollection<string> responsibilities, ICollection<string> requirements,
        ICollection<string> advantages, SalaryRecord salaryRecord, EmploymentTypeRecord employmentTypeRecord)
    {
        CategoryId = categoryId;
        JobFolderId = jobFolderId;
        Title = title;
        Description = description;
        IsPublic = isPublic;
        DateTimePublishedUtc = dateTimePublishedUtc;
        DateTimeExpiringUtc = dateTimeExpiringUtc;
        Responsibilities = responsibilities.ToList();
        Requirements = requirements.ToList();
        Advantages = advantages.ToList();
        SalaryRecord = salaryRecord;
        EmploymentTypeRecord = employmentTypeRecord;
    }
    
    public long Id { get; private set; }

    public byte[] RowVersion { get; set; } = [];

    public long CategoryId { get; set; }
    
    public long JobFolderId { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DateTimePublishedUtc { get; set; }

    public DateTime DateTimeExpiringUtc { get; set; }

    public SalaryRecord? SalaryRecord { get; set; }

    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }

    public ICollection<string>? Responsibilities { get; set; }

    public ICollection<string>? Requirements { get; set; }

    public ICollection<string>? Advantages { get; set; }

    public bool IsPublic { get; set; }
    
    public Category? Category { get; set; }
    public JobFolder? JobFolder { get; set; }
    public ICollection<JobApplication>? JobApplications { get; set; }
    public ICollection<JobContractType>? JobContractTypes { get; set; }
    public ICollection<Location>? Locations { get; set; }

    public ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
}