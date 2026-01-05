using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Categories;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.EmploymentOptions;
using JobSearchSiteBackend.Core.Domains.JobApplications;
using JobSearchSiteBackend.Core.Domains.JobContractTypes;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.Jobs;

public class Job : IEntityWithId, IEntityWithSearchSync
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Job() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Job(long categoryId, long companyId, string title, string? description, bool isPublic,
        DateTime dateTimePublishedUtc, DateTime dateTimeExpiringUtc, ICollection<string> responsibilities,
        ICollection<string> requirements, ICollection<string> niceToHaves, JobSalaryInfo? salaryInfo,
        ICollection<EmploymentOption> employmentOptions)
    {
        CategoryId = categoryId;
        CompanyId = companyId;
        Title = title;
        Description = description;
        IsPublic = isPublic;
        DateTimePublishedUtc = dateTimePublishedUtc;
        DateTimeExpiringUtc = dateTimeExpiringUtc;
        Responsibilities = responsibilities.ToList();
        Requirements = requirements.ToList();
        NiceToHaves = niceToHaves.ToList();
        SalaryInfo = salaryInfo;
        EmploymentOptions = employmentOptions;
    }
    
    public long Id { get; private set; }

    public DateTime DateTimeUpdatedUtc { get; set; }
    
    public DateTime? DateTimeSyncedWithSearchUtc { get; set; }
    
    public bool IsDeleted { get; set; }

    public long CategoryId { get; set; }

    public long CompanyId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime DateTimePublishedUtc { get; set; }

    public DateTime DateTimeExpiringUtc { get; set; }

    public JobSalaryInfo? SalaryInfo { get; set; }

    public ICollection<EmploymentOption>? EmploymentOptions { get; set; }

    public ICollection<string>? Responsibilities { get; set; }

    public ICollection<string>? Requirements { get; set; }

    public ICollection<string>? NiceToHaves { get; set; }

    public bool IsPublic { get; set; }

    public Category? Category { get; set; }
    public Company? Company { get; set; }
    public ICollection<JobApplication>? JobApplications { get; set; }
    public ICollection<JobContractType>? JobContractTypes { get; set; }
    public ICollection<Location>? Locations { get; set; }

    public ICollection<UserJobBookmark>? UserJobBookmarks { get; set; }
}