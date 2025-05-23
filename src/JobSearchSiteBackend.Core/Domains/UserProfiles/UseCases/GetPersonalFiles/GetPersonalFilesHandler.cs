﻿using Ardalis.Result;
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
    : IRequestHandler<GetPersonalFilesRequest, Result<GetPersonalFilesResponse>>
{
    public async Task<Result<GetPersonalFilesResponse>> Handle(GetPersonalFilesRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var query = context.PersonalFiles
            .Where(pf => pf.UserId == currentAccountId);
        
        var count = await query.CountAsync(cancellationToken);
        
        var personalFileInfoDtos = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ProjectTo<PersonalFileInfoDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize, count);
        
        var response = new GetPersonalFilesResponse(personalFileInfoDtos, paginationResponse);

        return response;
    }
}