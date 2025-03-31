using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobApplications.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplicationsForUser;

public class GetJobApplicationsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper)
    : IRequestHandler<GetJobApplicationsForUserRequest, Result<GetJobApplicationsForUserResponse>>
{
    public async Task<Result<GetJobApplicationsForUserResponse>> Handle(GetJobApplicationsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        if (currentAccountId != request.UserId)
            return Result<GetJobApplicationsForUserResponse>.Forbidden();

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

        var response = new GetJobApplicationsForUserResponse(jobApplicationDtos, paginationResponse);

        return response;
    }
}