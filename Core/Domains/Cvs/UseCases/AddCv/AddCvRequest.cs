using Core.Domains._Shared.UseCaseStructure;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.ValueEntities;
using Ardalis.Result;

namespace Core.Domains.Cvs.UseCases.AddCv;

public record AddCvRequest(long UserId, SalaryRecord? SalaryRecord, EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<EducationRecord> EducationRecords, ICollection<WorkRecord> WorkRecords,
    ICollection<string> Skills, ICollection<long>? CategoryIds) : IRequest<Result>;