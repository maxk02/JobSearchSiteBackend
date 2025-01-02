using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.ValueEntities;

namespace Core.Domains.Cvs.UseCases.GetCvById;

public record GetCvByIdResponse(long Id, SalaryRecord? SalaryRecord, EmploymentTypeRecord? EmploymentTypeRecord,
    IReadOnlyCollection<EducationRecord> EducationRecords, IReadOnlyCollection<WorkRecord> WorkRecords,
    IReadOnlyCollection<string> Skills, ICollection<long> CategoryIds);