using Application.Services.Search.Common;
using Domain.Entities.Users.ValueEntities;

namespace Application.Services.Search.Application;

public class ApplicationSearchModel : BaseSearchModel
{
    public int JobId { get; set; }
    public IList<EducationRecord> EducationList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public string? Description { get; set; }
}