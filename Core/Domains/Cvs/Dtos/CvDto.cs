using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.ValueEntities;

namespace Core.Domains.Cvs.Dtos;

public record CvDto(long Id, long UserId, SalaryRecord? SalaryRecord, EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<EducationRecord> EducationRecords, ICollection<WorkRecord> WorkRecords,
    ICollection<string> Skills);