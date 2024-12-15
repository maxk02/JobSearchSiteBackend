using Core.Domains.Cvs.ValueEntities;
using Core.Services.Search.Common;

namespace Core.Services.Search.Application;

public class ApplicationSearchModel : BaseSearchModel
{
    public int JobId { get; set; }
    public IList<EducationRecord> EducationList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public string? Description { get; set; }
}