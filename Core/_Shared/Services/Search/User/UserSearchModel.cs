using Core._Shared.Services.Search.Common;
using Core._Shared.ValueEntities;
using Core.Cvs.ValueEntities;

namespace Core._Shared.Services.Search.User;

public class UserSearchModel : BaseSearchModel
{
    public IList<EducationRecord> EducationRecordList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
}