using Core.Domains._Shared.Search;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.UserProfiles.ValueEntities;

namespace Core.Domains.Cvs.Search;

public record CvSearchModel(
    long Id, long UserId,
    SalaryRecord? SalaryRecord,
    EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<EducationRecord> EducationRecords,
    ICollection<WorkRecord> WorkRecords,
    ICollection<string> Skills,
    ICollection<long> AppliedToJobIds
) : ISearchModel;