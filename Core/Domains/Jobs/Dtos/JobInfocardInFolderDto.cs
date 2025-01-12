using Core.Domains._Shared.ValueEntities;

namespace Core.Domains.Jobs.Dtos;

public record JobInfocardInFolderDto(long Id, long CategoryId, string Title, DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc, SalaryRecord? SalaryRecord, EmploymentTypeRecord? EmploymentTypeRecord);