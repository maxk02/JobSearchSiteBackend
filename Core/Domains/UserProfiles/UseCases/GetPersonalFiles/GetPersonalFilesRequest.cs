using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetPersonalFiles;

public record GetPersonalFilesRequest(long UserId, PaginationSpec PaginationSpec) 
    : IRequest<Result<GetPersonalFilesResponse>>;