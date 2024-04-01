using System.Collections;
using Domain.Common;
using Domain.JSONEntities;

namespace Domain.Entities;

public class UserDataSet : BaseEntity, IHideableEntity
{
    public string FirstName { get; set; } = "";
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = "";
    public DateOnly? DateOfBirth { get; set; }
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    
    public int? MinSalary { get; set; }
    public List<EducationRecord> EducationRecordList { get; set; } = [];
    public List<WorkRecord> WorkRecordList { get; set; } = [];
    public List<string> SkillList { get; set; } = [];
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