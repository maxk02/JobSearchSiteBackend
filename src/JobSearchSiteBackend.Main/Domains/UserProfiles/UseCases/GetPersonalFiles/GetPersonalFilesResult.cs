using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetPersonalFiles;

public record GetPersonalFilesResult(
    ICollection<PersonalFileInfoDto> PersonalFileInfos,
    PaginationResponse PaginationResponse);