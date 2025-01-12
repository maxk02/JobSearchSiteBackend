using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobApplications.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplicationsForUser;

public class GetJobApplicationsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
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
            .Where(jobApplication => jobApplication.UserId == currentAccountId)
            .Select(jobApplication => new JobApplicationInUserProfileDto(
                jobApplication.Id,
                jobApplication.Job!.JobFolder!.CompanyId,
                jobApplication.Job!.JobFolder!.Company!.Name,
                jobApplication.JobId,
                jobApplication.Job.Title,
                jobApplication.Job.DateTimePublishedUtc,
                jobApplication.Job.SalaryRecord,
                jobApplication.Job.EmploymentTypeRecord,
                jobApplication.DateTimeCreatedUtc,
                jobApplication.Status.ToString()));

        var count = await query.CountAsync(cancellationToken);

        var jobApplicationDtos = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ToListAsync(cancellationToken);


        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);

        var response = new GetJobApplicationsForUserResponse(jobApplicationDtos, paginationResponse);

        return response;
    }
}