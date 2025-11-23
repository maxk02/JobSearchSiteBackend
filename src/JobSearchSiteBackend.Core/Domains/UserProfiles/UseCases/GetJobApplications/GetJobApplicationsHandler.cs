using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetJobApplications;

public class GetJobApplicationsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper)
    : IRequestHandler<GetJobApplicationsQuery, Result<GetJobApplicationsResult>>
{
    public async Task<Result<GetJobApplicationsResult>> Handle(GetJobApplicationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.JobApplications
            .AsNoTracking()
            .Where(jobApplication => jobApplication.UserId == currentAccountId);

        var count = await dbQuery.CountAsync(cancellationToken);

        var jobApplicationDtos = await dbQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ProjectTo<JobApplicationInUserProfileDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var paginationResponse = new PaginationResponse(query.Page,
            query.Size, count);

        var response = new GetJobApplicationsResult(jobApplicationDtos, paginationResponse);

        return response;
    }
}