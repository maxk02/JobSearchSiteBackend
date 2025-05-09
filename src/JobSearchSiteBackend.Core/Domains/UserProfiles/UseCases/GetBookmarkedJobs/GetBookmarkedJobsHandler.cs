using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;

public class GetBookmarkedJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper)
    : IRequestHandler<GetBookmarkedJobsRequest, Result<GetBookmarkedJobsResponse>>
{
    public async Task<Result<GetBookmarkedJobsResponse>> Handle(GetBookmarkedJobsRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var query = context.UserProfiles
            .Include(u => u.BookmarkedJobs)!
            .ThenInclude(job => job.JobFolder)
            .Where(u => u.Id == currentAccountId)
            .SelectMany(u => u.BookmarkedJobs ?? new List<Job>());

        var count = await query.CountAsync(cancellationToken);

        var bookmarkedJobCardDtos = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ProjectTo<JobCardDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize, count);

        var response = new GetBookmarkedJobsResponse(bookmarkedJobCardDtos, paginationResponse);

        return response;
    }
}