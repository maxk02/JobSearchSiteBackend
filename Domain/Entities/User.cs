using System.Collections;
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
    
    public SalaryRecord? SalaryExpectation { get; set; }
    public IList<EducationRecord> EducationRecordList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsHidden { get; set; }
    
    public virtual IList<Location>? Locations { get; set; }
    public virtual IList<Category>? Categories { get; set; }
    
    public virtual IList<UserJobBookmark>? UserJobBookmarks { get; set; }
    public virtual IList<UserJobPermissionSet>? UserJobPermissionSets { get; set; }
    
    public virtual IList<UserCompanyBookmark>? UserCompanyBookmarks { get; set; }
    public virtual IList<UserCompanyPermissionSet>? UserCompanyPermissionSets { get; set; }
    
    public virtual IList<UserTagPermissionSet>? UserTagPermissionSets { get; set; }
}