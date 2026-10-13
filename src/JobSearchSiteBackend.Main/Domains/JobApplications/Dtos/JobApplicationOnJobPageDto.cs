using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

public record JobApplicationOnJobPageDto(
    long Id,
    LocationDto LocationDto,
    DateTime DateTimeAppliedUtc,
    ICollection<long> PersonalFileIds,
    int StatusId);