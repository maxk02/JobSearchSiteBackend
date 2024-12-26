using Core.Domains._Shared.UseCaseStructure;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.UserProfiles.ValueEntities;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.UpdateCv;

public record UpdateCvRequest(SalaryRecord? SalaryRecord, EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<EducationRecord>? EducationRecords, ICollection<WorkRecord>? WorkRecords,
    ICollection<string>? Skills) : IRequest<Result>;