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
    
    public int? MinSalary { get; set; }
    public List<Education> EducationList { get; set; } = [];
    public List<WorkRecord> WorkRecordList { get; set; } = [];
    public List<string> SkillList { get; set; } = [];
    public EmploymentType? EmploymentType { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsHidden { get; set; }
}