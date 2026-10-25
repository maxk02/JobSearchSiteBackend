using JobSearchSiteBackend.Main.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Main.Domains.Locations.Dtos;
using JobSearchSiteBackend.Main.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.Dtos;

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