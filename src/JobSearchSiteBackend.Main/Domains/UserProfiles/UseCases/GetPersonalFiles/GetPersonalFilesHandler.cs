using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.PersonalFiles;
using JobSearchSiteBackend.Main.Domains.PersonalFiles.Dtos;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetPersonalFiles;

public class GetPersonalFilesHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) 
    : IRequestHandler<GetPersonalFilesQuery, Result<GetPersonalFilesResult>>
{
    public async Task<Result<GetPersonalFilesResult>> Handle(GetPersonalFilesQuery query,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.PersonalFiles
            .Where(pf => pf.UserId == currentAccountId);
        
        var count = await dbQuery.CountAsync(cancellationToken);
        
        var personalFiles = await dbQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        var personalFileInfoDtos = personalFiles
            .Select(pf => pf.ToPersonalFileInfoDto())
            .ToList();

        var paginationResponse = new PaginationResponse(query.Page,
            query.Size, count);
        
        var response = new GetPersonalFilesResult(personalFileInfoDtos, paginationResponse);

        return response;
    }
}