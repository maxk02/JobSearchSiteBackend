using Application.Services.Search.Common;
using Domain.JSONEntities;

namespace Application.Services.Search.Application;

public class ApplicationSearchModel : BaseSearchModel
{
    public long JobId { get; set; }
    public List<EducationRecord> EducationList { get; set; } = [];
    public List<WorkRecord> WorkRecordList { get; set; } = [];
    public List<string> SkillList { get; set; } = [];
    public string? Description { get; set; }
}