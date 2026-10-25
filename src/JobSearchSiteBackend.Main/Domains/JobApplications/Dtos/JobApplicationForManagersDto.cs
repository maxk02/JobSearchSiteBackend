using JobSearchSiteBackend.Main.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Main.Domains.Locations.Dtos;
using JobSearchSiteBackend.Main.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.Dtos;

public record JobApplicationForManagersDto(
    long Id,
    long UserId,
    string UserFullName,
    string? AvatarLink,
    string Email,
    string? Phone,
    ICollection<string> Tags,
    DateTime DateTimeAppliedUtc,
    ICollection<PersonalFileInfoDto> PersonalFiles,
    int Status,
    LocationDto LocationDto);