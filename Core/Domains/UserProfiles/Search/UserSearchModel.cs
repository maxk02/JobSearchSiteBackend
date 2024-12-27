using Core.Domains._Shared.Search;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.UserProfiles.ValueEntities;

namespace Core.Domains.UserProfiles.Search;

public class UserSearchModel : SearchModelBase
{
    public IList<EducationRecord> EducationRecordList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
}