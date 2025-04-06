using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.UserProfiles.UseCases.GetPersonalFiles;

public record GetPersonalFilesRequest(PaginationSpec PaginationSpec) 
    : IRequest<Result<GetPersonalFilesResponse>>;