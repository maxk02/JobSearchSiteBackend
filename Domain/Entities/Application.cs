using Domain.Common;
using Domain.JSONEntities;

namespace Domain.Entities;

public class Application : BaseEntity
{
    public virtual UserDataSet? User { get; set; }
    public Guid UserId { get; set; }
    
    public virtual Job? Job { get; set; }
    public Guid JobId { get; set; }
    
    public int? MinSalary { get; set; }
    public List<EducationRecord> EducationList { get; set; } = [];
    public List<WorkRecord> WorkRecordList { get; set; } = [];
    public List<string> SkillList { get; set; } = [];
    public string? Description { get; set; }
    public EmploymentTypeRecord? EmploymentType { get; set; }
    
    public virtual IList<File>? Files { get; set; }
}