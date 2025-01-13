using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobContractTypes.Dtos;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Locations.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Jobs.UseCases.GetJobById;

public class GetJobByIdHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetJobByIdRequest, Result<GetJobByIdResponse>>
{
    public async Task<Result<GetJobByIdResponse>> Handle(GetJobByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var job = await context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == request.Id)
            .Include(job => job.SalaryRecord)
            .Include(job => job.EmploymentTypeRecord)
            .Include(job => job.Responsibilities)
            .Include(job => job.Requirements)
            .Include(job => job.Advantages)
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
                    JobFolderClaim.CanEditJobsAndSubfolders.Id)
                .AnyAsync(cancellationToken);

            if (!canEdit)
                return Result<GetJobByIdResponse>.Forbidden();
        }

        var jobContractTypeDtos = job.JobContractTypes!
            .Select(jct => new JobContractTypeDto(jct.Id, jct.Name))
            .ToArray();

        var locationDtos = job.Locations!
            .Select(l => new LocationDto(l.Id, l.CountryId, l.Name,
                l.Subdivisions, l.Description, l.Code))
            .ToArray();

        return new GetJobByIdResponse(job.Id, job.CategoryId, job.Title, job.Description,
            job.DateTimePublishedUtc, job.DateTimeExpiringUtc, job.Responsibilities ?? [],
            job.Requirements ?? [], job.Advantages ?? [], job.SalaryRecord, job.EmploymentTypeRecord,
            jobContractTypeDtos, locationDtos);
    }
}