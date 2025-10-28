using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobManagementDto;

public class GetJobManagementDtoHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetJobManagementDtoRequest, Result<GetJobManagementDtoResponse>>
{
    public async Task<Result<GetJobManagementDtoResponse>> Handle(GetJobManagementDtoRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetId();

        if (currentUserId is null)
            return Result.Forbidden();
        
        var job = await context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == request.Id)
            .Include(job => job.JobFolder)
            .ThenInclude(jf => jf!.Company)
            .Include(job => job.SalaryInfo)
            .Include(job => job.EmploymentTypes)
            .Include(job => job.Responsibilities)
            .Include(job => job.Requirements)
            .Include(job => job.NiceToHaves)
            .Include(job => job.JobContractTypes)
            .Include(job => job.Locations)
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            return Result<GetJobManagementDtoResponse>.NotFound();

        var claimIdsForCurrentUser = context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(job.JobFolderId, currentUserId.Value)
            .ToList();

        if (!claimIdsForCurrentUser.Contains(JobFolderClaim.CanEditJobs.Id))
            return Result.Forbidden();

        var jobManagementDto = new JobManagementDto(
            job.Id,
            job.JobFolder!.CompanyId,
            "", // todo
            job.JobFolder!.Company!.Name,
            job.JobFolder.Company.Description,
            mapper.Map<IList<LocationDto>>(job.Locations),
            job.CategoryId,
            job.Title,
            job.Description,
            job.DateTimePublishedUtc,
            job.DateTimeExpiringUtc,
            job.Responsibilities!,
            job.Requirements!,
            job.NiceToHaves!,
            mapper.Map<JobSalaryInfoDto>(job.SalaryInfo),
            job.EmploymentTypes!.Select(x => x.Id).ToList(),
            job.JobContractTypes!.Select(x => x.Id).ToList(),
            job.JobFolderId,
            job.JobFolder.Name!,
            claimIdsForCurrentUser,
            job.IsPublic,
            job.TimeRangeOptionId
            );
        
        return new GetJobManagementDtoResponse(jobManagementDto);
    }
}