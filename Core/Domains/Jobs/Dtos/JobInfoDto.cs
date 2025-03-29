using Core.Domains._Shared.ValueEntities;

namespace Core.Domains.Jobs.Dtos;

public record JobInfoDto(long Id, long CompanyId, long CategoryId, string Title, DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc, SalaryRecord? SalaryRecord, EmploymentTypeRecord? EmploymentTypeRecord);