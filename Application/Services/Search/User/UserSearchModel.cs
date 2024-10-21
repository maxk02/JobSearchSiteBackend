using Application.Services.Search.Common;
using Domain.Entities.Users.ValueEntities;
using Domain.ValueObjects;

namespace Application.Services.Search.User;

public class UserSearchModel : BaseSearchModel
{
    public IList<EducationRecord> EducationRecordList { get; set; } = [];
    public IList<WorkRecord> WorkRecordList { get; set; } = [];
    public IList<string> SkillList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
}