using Core.Domains._Shared.Pagination;
using Core.Domains.PersonalFiles.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetPersonalFiles;

public record GetPersonalFilesResponse(
    ICollection<PersonalFileInfocardDto> PersonalFileInfocardDtos,
    PaginationResponse PaginationResponse);