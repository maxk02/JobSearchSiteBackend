using Core.Domains._Shared.UseCaseStructure;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.ValueEntities;
using Ardalis.Result;

namespace Core.Domains.Cvs.UseCases.UpdateCv;

public record UpdateCvRequest(long CvId, SalaryRecord? SalaryRecord, EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<EducationRecord>? EducationRecords, ICollection<WorkRecord>? WorkRecords,
    ICollection<string>? Skills, ICollection<long>? CategoryIds) : IRequest<Result>;