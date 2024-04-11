using Domain.Common;
using Domain.JSONEntities;

namespace Domain.Entities;

public class MyApplication : BaseEntity
{
    public virtual User? User { get; set; }
    public long UserId { get; set; }
    
    public virtual Job? Job { get; set; }
    public long JobId { get; set; }
    
    public SalaryRecord? SalaryExpectation { get; set; }
    public IList<EducationRecord> EducationList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public string? Description { get; set; }
    public EmploymentTypeRecord? EmploymentType { get; set; }
    
    public virtual IList<MyFile>? Files { get; set; }
}