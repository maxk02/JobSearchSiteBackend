using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.ValueEntities;

namespace Core.Domains.Cvs.UseCases.GetCvById;

public record GetCvByIdResponse(long Id, SalaryRecord? SalaryRecord, EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<EducationRecord> EducationRecords, ICollection<WorkRecord> WorkRecords,
    ICollection<string> Skills, ICollection<long> CategoryIds);