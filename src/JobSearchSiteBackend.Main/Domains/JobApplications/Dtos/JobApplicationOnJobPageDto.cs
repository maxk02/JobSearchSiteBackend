using JobSearchSiteBackend.Main.Domains.Locations.Dtos;
using JobSearchSiteBackend.Main.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.Dtos;

public record JobApplicationOnJobPageDto(
    long Id,
    LocationDto LocationDto,
    DateTime DateTimeAppliedUtc,
    ICollection<long> PersonalFileIds,
    int StatusId);