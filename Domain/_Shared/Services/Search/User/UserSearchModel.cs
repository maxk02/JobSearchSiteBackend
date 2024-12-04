using Domain._Shared.Services.Search.Common;
using Domain._Shared.ValueEntities;
using Domain.Cvs.ValueEntities;

namespace Domain._Shared.Services.Search.User;

public class UserSearchModel : BaseSearchModel
{
    public IList<EducationRecord> EducationRecordList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
}