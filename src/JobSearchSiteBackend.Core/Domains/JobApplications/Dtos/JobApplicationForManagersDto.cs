using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

public record JobApplicationForManagersDto(
    long Id,
    long UserId,
    string UserFullName,
    DateTime DateTimeAppliedUtc,
    ICollection<PersonalFileInfoDto> PersonalFiles,
    JobApplicationStatus Status);