using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.PersonalFiles;
using Core.Domains.PersonalFiles.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetPersonalFiles;

public class GetPersonalFilesHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) 
    : IRequestHandler<GetPersonalFilesRequest, Result<GetPersonalFilesResponse>>
{
    public async Task<Result<GetPersonalFilesResponse>> Handle(GetPersonalFilesRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        if (currentAccountId != request.UserId)
            return Result<GetPersonalFilesResponse>.Forbidden();

        var query = context.UserProfiles
            .Include(u => u.PersonalFiles)
            .Where(u => u.Id == currentAccountId)
            .SelectMany(u => u.PersonalFiles ?? new List<PersonalFile>());
        
        var count = await query.CountAsync(cancellationToken);
        
        var bookmarkedCompanies = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ToListAsync(cancellationToken);

        var personalFileInfocardDtos = bookmarkedCompanies
            .Select(x => new PersonalFileInfocardDto(x.Id, x.UserId, x.Name, x.Extension, x.Size))
            .ToList();

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);
        
        var response = new GetPersonalFilesResponse(personalFileInfocardDtos, paginationResponse);

        return response;
    }
}