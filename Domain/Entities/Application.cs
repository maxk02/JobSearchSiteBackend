using Domain.Common;
using Domain.JSONEntities;

namespace Domain.Entities;

public class Application : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid JobId { get; set; }
    public int? MinSalary { get; set; }
    public List<Education> EducationList { get; set; } = [];
    public List<WorkRecord> WorkRecordList { get; set; } = [];
    public List<string> Skills { get; set; } = [];
    public string? Description { get; set; }
    public EmploymentType? EmploymentType { get; set; }
}