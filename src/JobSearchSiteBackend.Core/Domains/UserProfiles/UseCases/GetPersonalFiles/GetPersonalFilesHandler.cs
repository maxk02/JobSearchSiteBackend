using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetPersonalFiles;

public class GetPersonalFilesHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) 
    : IRequestHandler<GetPersonalFilesQuery, Result<GetPersonalFilesResult>>
{
    public async Task<Result<GetPersonalFilesResult>> Handle(GetPersonalFilesQuery query,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.PersonalFiles
            .Where(pf => pf.UserId == currentAccountId);
        
        var count = await dbQuery.CountAsync(cancellationToken);
        
        var personalFileInfoDtos = await dbQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ProjectTo<PersonalFileInfoDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(query.Page,
            query.Size, count);
        
        var response = new GetPersonalFilesResult(personalFileInfoDtos, paginationResponse);

        return response;
    }
}