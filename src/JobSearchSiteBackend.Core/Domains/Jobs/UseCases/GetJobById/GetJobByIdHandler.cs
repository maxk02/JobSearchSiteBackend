using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobContractTypes.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobById;

public class GetJobByIdHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetJobByIdRequest, Result<GetJobByIdResponse>>
{
    public async Task<Result<GetJobByIdResponse>> Handle(GetJobByIdRequest request,
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
            return Result<GetJobByIdResponse>.NotFound();

        if (!job.IsPublic)
        {
            var currentUserId = currentAccountService.GetId();

            if (currentUserId is null)
                return Result<GetJobByIdResponse>.Forbidden();

            var canEdit = await context.JobFolderRelations
                .GetThisOrAncestorWhereUserHasClaim(job.JobFolderId, currentUserId.Value,
                    JobFolderClaim.CanEditJobs.Id)
                .AnyAsync(cancellationToken);

            if (!canEdit)
                return Result<GetJobByIdResponse>.Forbidden();
        }
        
        var jobDetailedDto = mapper.Map<JobDetailedDto>(job);
        
        return new GetJobByIdResponse(jobDetailedDto);
    }
}