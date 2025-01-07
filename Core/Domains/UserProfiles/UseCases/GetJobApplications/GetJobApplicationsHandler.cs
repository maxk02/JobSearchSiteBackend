using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobApplications;
using Core.Domains.JobApplications.Dtos;
using Core.Domains.PersonalFiles;
using Core.Domains.PersonalFiles.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplications;

public class GetJobApplicationsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) 
    : IRequestHandler<GetJobApplicationsRequest, Result<GetJobApplicationsResponse>>
{
    public async Task<Result<GetJobApplicationsResponse>> Handle(GetJobApplicationsRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        if (currentAccountId != request.UserId)
            return Result<GetJobApplicationsResponse>.Forbidden();

        var query = context.UserProfiles
            .Include(u => u.JobApplications)
            .Where(u => u.Id == currentAccountId)
            .SelectMany(u => u.JobApplications ?? new List<JobApplication>());
        
        var count = await query.CountAsync(cancellationToken);
        
        var bookmarkedCompanies = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ToListAsync(cancellationToken);

        var jobApplicationDtos = bookmarkedCompanies
            .Select(x => new JobApplicationDto(x.Id, x.UserId, x.JobId, x.Status))
            .ToList();

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);
        
        var response = new GetJobApplicationsResponse(jobApplicationDtos, paginationResponse);

        return response;
    }
}