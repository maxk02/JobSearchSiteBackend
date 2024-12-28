using Core.Domains._Shared.Search;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.UserProfiles.ValueEntities;

namespace Core.Domains.Cvs.Search;

public class CvSearchModel : SearchModelBase
{
    public required long UserId { get; set; }
    
    public SalaryRecord? SalaryRecord { get; set; }

    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }

    public ICollection<EducationRecord> EducationRecords { get; set; } = [];

    public ICollection<WorkRecord> WorkRecords { get; set; } = [];

    public ICollection<string> Skills { get; set; } = [];
    
    public ICollection<long> AppliedToJobIds { get; set; } = [];

}