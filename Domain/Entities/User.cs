using Domain.Common;
using Domain.JSONEntities;

namespace Domain.Entities;

public class User : BaseEntity, IHideableEntity
{
    public string FirstName { get; set; } = "";
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = "";
    public DateOnly? DateOfBirth { get; set; }
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    
    public SalaryRecord? SalaryRecord { get; set; }
    public IList<EducationRecord> EducationRecords { get; set; } = [];
    public IList<WorkRecord> WorkRecords { get; set; } = [];
    public IList<string> Skills { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsHidden { get; set; }
    
    public virtual IList<Location>? Locations { get; set; }
    public virtual IList<Category>? Categories { get; set; }
    public virtual IList<MyFile>? MyFiles { get; set; }
    public virtual IList<MyApplication>? MyApplications { get; set; }
    
    public virtual IList<UserJobBookmark>? UserJobBookmarks { get; set; }
    
    public virtual IList<UserCompanyBookmark>? UserCompanyBookmarks { get; set; }
    public virtual IList<UserCompanyPermissionSet>? UserCompanyPermissionSets { get; set; }
    
    public virtual IList<UserTagPermissionSet>? UserTagPermissionSets { get; set; }
}