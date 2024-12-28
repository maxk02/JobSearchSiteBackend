using Core.Domains._Shared.Search;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.UserProfiles.ValueEntities;

namespace Core.Domains.UserProfiles.Search;

public class CvSearchModel : SearchModelBase
{
    public SalaryRecord? SalaryRecord { get; private set; }

    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }

    public ICollection<EducationRecord> EducationRecords { get; set; } = [];

    public ICollection<WorkRecord> WorkRecords { get; set; } = [];

    public ICollection<string> Skills { get; set; } = [];
    
    public ICollection<long> AppliedToJobIds { get; set; } = [];

}