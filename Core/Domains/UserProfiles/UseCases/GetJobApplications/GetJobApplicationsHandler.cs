using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobApplications.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplications;

public class GetJobApplicationsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper)
    : IRequestHandler<GetJobApplicationsRequest, Result<GetJobApplicationsResponse>>
{
    public async Task<Result<GetJobApplicationsResponse>> Handle(GetJobApplicationsRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        if (currentAccountId != request.Id)
            return Result<GetJobApplicationsResponse>.Forbidden();

        var query = context.JobApplications
            .AsNoTracking()
            .Where(jobApplication => jobApplication.UserId == currentAccountId);

        var count = await query.CountAsync(cancellationToken);

        var jobApplicationDtos = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ProjectTo<JobApplicationInUserProfileDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);

        var response = new GetJobApplicationsResponse(jobApplicationDtos, paginationResponse);

        return response;
    }
}