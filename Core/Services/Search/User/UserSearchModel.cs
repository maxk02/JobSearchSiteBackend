using Core.Domains._Shared.ValueEntities;
using Core.Domains.UserProfiles.ValueEntities;
using Core.Services.Search.Common;

namespace Core.Services.Search.User;

public class UserSearchModel : BaseSearchModel
{
    public IList<EducationRecord> EducationRecordList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
}