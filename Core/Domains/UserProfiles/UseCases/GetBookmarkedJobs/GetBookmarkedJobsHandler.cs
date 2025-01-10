using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Jobs;
using Core.Domains.Jobs.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;

public class GetBookmarkedJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetBookmarkedJobsRequest, Result<GetBookmarkedJobsResponse>>
{
    public async Task<Result<GetBookmarkedJobsResponse>> Handle(GetBookmarkedJobsRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        if (currentAccountId != request.UserId)
            return Result<GetBookmarkedJobsResponse>.Forbidden();

        var query = context.UserProfiles
            .Include(u => u.BookmarkedJobs)!
            .ThenInclude(job => job.JobFolder)
            .Where(u => u.Id == currentAccountId)
            .SelectMany(u => u.BookmarkedJobs ?? new List<Job>());

        var count = await query.CountAsync(cancellationToken);

        var bookmarkedJobs = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ToListAsync(cancellationToken);

        var jobInfocardDtos = bookmarkedJobs
            .Select(x => new JobInfocardDto(x.Id, x.JobFolder!.CompanyId, x.CategoryId, x.Title,
                x.DateTimePublishedUtc, x.DateTimeExpiringUtc, x.SalaryRecord, x.EmploymentTypeRecord))
            .ToList();

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);

        var response = new GetBookmarkedJobsResponse(jobInfocardDtos, paginationResponse);

        return response;
    }
}