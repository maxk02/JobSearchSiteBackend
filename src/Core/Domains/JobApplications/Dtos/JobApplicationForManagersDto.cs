using Core.Domains.JobApplications.Enums;
using Core.Domains.PersonalFiles.Dtos;

namespace Core.Domains.JobApplications.Dtos;

public record JobApplicationForManagersDto(
    long Id,
    long UserId,
    string UserFullName,
    DateTime DateTimeAppliedUtc,
    ICollection<PersonalFileInfoDto> PersonalFiles,
    JobApplicationStatus Status);