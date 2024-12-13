using API.Domains._Shared.ValueEntities;
using API.Domains.Cvs.ValueEntities;
using API.Services.Search.Common;

namespace API.Services.Search.User;

public class UserSearchModel : BaseSearchModel
{
    public IList<EducationRecord> EducationRecordList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
}