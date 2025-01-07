using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Domains.Cvs.Search;
using Core.Domains.Cvs.ValueEntities;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Shared.Result;

namespace Core.Domains.Cvs.UseCases.AddCv;

public class AddCvHandler(
    ICurrentAccountService currentAccountService,
    ICvSearchRepository cvSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<AddCvRequest, Result>
{
    public async Task<Result> Handle(AddCvRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (currentUserId != request.UserId)
            return Result.Forbidden();

        var newCvCreationResult = Cv.Create(
            request.UserId,
            request.SalaryRecord,
            request.EmploymentTypeRecord,
            request.EducationRecords,
            request.WorkRecords,
            request.Skills
        );

        if (newCvCreationResult.IsFailure)
            return newCvCreationResult;

        var newCv = newCvCreationResult.Value;

        if (request.CategoryIds is not null)
        {
            newCv.Categories = Category.AllValues
                .Where(c => request.CategoryIds.Contains(c.Id))
                .ToList();
        }

        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        context.Cvs.Add(newCv);
        await context.SaveChangesAsync(cancellationToken);

        var cvSearchModel = new CvSearchModel(newCv.Id, newCv.UserId,
            newCv.EducationRecords ?? new List<EducationRecord>(),
            newCv.WorkRecords ?? new List<WorkRecord>(),
            newCv.Skills ?? new List<string>(),
            new List<long>(), new List<long>());

        await transaction.CommitAsync(cancellationToken);
        
        backgroundJobService.Enqueue(
            () => cvSearchRepository.AddAsync(cvSearchModel, CancellationToken.None),
            BackgroundJobQueues.CvSearch
        );
        
        return Result.Success();
    }
}