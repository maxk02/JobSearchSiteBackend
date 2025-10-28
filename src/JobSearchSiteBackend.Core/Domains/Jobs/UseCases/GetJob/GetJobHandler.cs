using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;

public class GetJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetJobRequest, Result<GetJobResponse>>
{
    public async Task<Result<GetJobResponse>> Handle(GetJobRequest request,
        CancellationToken cancellationToken = default)
    {
        var job = await context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == request.Id)
            .Include(job => job.SalaryInfo)
            .Include(job => job.EmploymentTypes)
            .Include(job => job.Responsibilities)
            .Include(job => job.Requirements)
            .Include(job => job.NiceToHaves)
            .Include(job => job.JobContractTypes)
            .Include(job => job.Locations)
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            return Result<GetJobResponse>.NotFound();

        // if (!job.IsPublic)
        // {
        //     var currentUserId = currentAccountService.GetId();
        //
        //     if (currentUserId is null)
        //         return Result<GetJobResponse>.Forbidden();
        //
        //     var canEdit = await context.JobFolderRelations
        //         .GetThisOrAncestorWhereUserHasClaim(job.JobFolderId, currentUserId.Value,
        //             JobFolderClaim.CanEditJobs.Id)
        //         .AnyAsync(cancellationToken);
        //
        //     if (!canEdit)
        //         return Result<GetJobResponse>.Forbidden();
        // }
        
        if (!job.IsPublic)
            return Result.Forbidden();
        
        var jobDetailedDto = mapper.Map<JobDetailedDto>(job);
        
        return new GetJobResponse(jobDetailedDto);
    }
}