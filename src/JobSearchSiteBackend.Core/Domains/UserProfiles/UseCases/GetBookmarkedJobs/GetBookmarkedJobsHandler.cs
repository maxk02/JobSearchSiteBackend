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
    : IRequestHandler<GetBookmarkedJobsQuery, Result<GetBookmarkedJobsResult>>
{
    public async Task<Result<GetBookmarkedJobsResult>> Handle(GetBookmarkedJobsQuery query,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.UserProfiles
            .Include(u => u.BookmarkedJobs)!
            .ThenInclude(job => job.JobFolder)
            .Where(u => u.Id == currentAccountId)
            .SelectMany(u => u.BookmarkedJobs ?? new List<Job>());

        var count = await dbQuery.CountAsync(cancellationToken);

        var bookmarkedJobCardDtos = await dbQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ProjectTo<JobCardDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(query.Page,
            query.Size, count);

        var response = new GetBookmarkedJobsResult(bookmarkedJobCardDtos, paginationResponse);

        return response;
    }
}