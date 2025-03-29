using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.Dtos;
using Core.Domains.PersonalFiles.Dtos;

namespace Core.Domains.JobApplications.Dtos;

public record JobApplicationForManagersDto(
    long Id,
    long UserProfileId,
    string UserFullName,
    DateTime DateTimeAppliedUtc,
    CvDto? Cv,
    ICollection<PersonalFileInfoDto> PersonalFiles,
    string Status);