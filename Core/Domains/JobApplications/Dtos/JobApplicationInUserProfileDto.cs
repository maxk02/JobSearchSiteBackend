using Core.Domains._Shared.ValueEntities;

namespace Core.Domains.JobApplications.Dtos;

public record JobApplicationInUserProfileDto(
    long Id,
    long CompanyId,
    string CompanyName,
    long JobId,
    string JobTitle,
    DateTime DateTimePublishedUtc,
    SalaryRecord? SalaryRecord,
    EmploymentTypeRecord? EmploymentTypeRecord,
    DateTime DateTimeAppliedUtc,
    string Status);