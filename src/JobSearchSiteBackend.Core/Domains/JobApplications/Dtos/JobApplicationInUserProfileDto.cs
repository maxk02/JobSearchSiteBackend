using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

public record JobApplicationInUserProfileDto(
    long Id,
    long CompanyId,
    string CompanyName,
    string? CompanyAvatarLink,
    long JobId,
    string JobTitle,
    DateTime DateTimePublishedUtc,
    JobSalaryInfoDto? JobSalaryInfoDto,
    LocationDto LocationDto, //todo
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? ContractTypeIds,
    DateTime DateTimeAppliedUtc,
    ICollection<PersonalFileInfoDto> PersonalFileInfoDtos,
    int Status);