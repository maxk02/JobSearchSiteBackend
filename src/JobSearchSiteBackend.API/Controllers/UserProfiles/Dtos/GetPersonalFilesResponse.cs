using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record GetPersonalFilesResponse(
    ICollection<PersonalFileInfoDto> PersonalFileInfos,
    PaginationResponse PaginationResponse);