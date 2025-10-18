using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Categories;
using JobSearchSiteBackend.Core.Domains.EmploymentOptions;
using JobSearchSiteBackend.Core.Domains.JobApplications;
using JobSearchSiteBackend.Core.Domains.JobContractTypes;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.Jobs;

public class Job : IEntityWithId
{
    public Job(long categoryId, long jobFolderId, string title, string description, bool isPublic,
        DateTime dateTimePublishedUtc,
        DateTime dateTimeExpiringUtc, ICollection<string> responsibilities, ICollection<string> requirements,
        ICollection<string> niceToHaves, JobSalaryInfo? salaryInfo, ICollection<EmploymentOption> employmentTypes)
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
        NiceToHaves = niceToHaves.ToList();
        SalaryInfo = salaryInfo;
        EmploymentTypes = employmentTypes;
    }
    
    public long Id { get; private set; }

    public DateTime? LastUpdatedUtc { get; private set; }
    
    public bool IsDeleted { get; private set; }

    public long CategoryId { get; set; }

    public long JobFolderId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime DateTimePublishedUtc { get; set; }

    public DateTime DateTimeExpiringUtc { get; set; }

    public JobSalaryInfo? SalaryInfo { get; set; }

    public ICollection<EmploymentOption>? EmploymentTypes { get; set; }

    public ICollection<string>? Responsibilities { get; set; }

    public ICollection<string>? Requirements { get; set; }

    public ICollection<string>? NiceToHaves { get; set; }

    public bool IsPublic { get; set; }

    public Category? Category { get; set; }
    public JobFolder? JobFolder { get; set; }
    public ICollection<JobApplication>? JobApplications { get; set; }
    public ICollection<JobContractType>? JobContractTypes { get; set; }
    public ICollection<Location>? Locations { get; set; }

    public ICollection<UserProfile>? UsersWhoBookmarked { get; set; }
}