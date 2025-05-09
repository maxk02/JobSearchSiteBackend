using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetPersonalFiles;

public record GetPersonalFilesResponse(
    ICollection<PersonalFileInfoDto> PersonalFileInfos,
    PaginationResponse PaginationResponse);