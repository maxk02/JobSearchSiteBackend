using Core.Domains._Shared.Search;
using Core.Domains.UserProfiles.ValueEntities;

namespace Core.Domains.JobApplications.Search;

public class ApplicationSearchModel : SearchModelBase
{
    public int JobId { get; set; }
    public IList<EducationRecord> EducationList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public string? Description { get; set; }
}