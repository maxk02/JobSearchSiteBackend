using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;

public class GetJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IPageVisitCacheRepository cacheRepo,
    IMapper mapper) : IRequestHandler<GetJobRequest, Result<GetJobResponse>>
{
    public async Task<Result<GetJobResponse>> Handle(GetJobRequest request,
        CancellationToken cancellationToken = default)
    {
        var job = await context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == request.Id)
            .Include(j => j.JobFolder)
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
        
        if (!job.IsPublic)
            return Result.Forbidden();

        await cacheRepo.IncrementJobVisitsCounterAsync(job.JobFolder!.CompanyId.ToString(), job.Id.ToString());
        
        var jobDetailedDto = mapper.Map<JobDetailedDto>(job);
        
        return new GetJobResponse(jobDetailedDto);
    }
}