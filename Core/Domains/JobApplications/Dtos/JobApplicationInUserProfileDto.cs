using Core.Domains.JobApplications.Enums;
using Core.Domains.Jobs.Dtos;

namespace Core.Domains.JobApplications.Dtos;

public record JobApplicationInUserProfileDto(
    long Id,
    long CompanyId,
    string CompanyName,
    long JobId,
    string JobTitle,
    DateTime DateTimePublishedUtc,
    JobSalaryInfoDto? JobSalaryInfoDto,
    ICollection<long>? EmploymentTypeIds,
    DateTime DateTimeAppliedUtc,
    JobApplicationStatus Status);