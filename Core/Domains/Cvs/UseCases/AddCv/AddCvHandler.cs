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

        var newCv = new Cv(
            request.UserId,
            request.SalaryRecord,
            request.EmploymentTypeRecord,
            request.EducationRecords,
            request.WorkRecords,
            request.Skills
        );

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

        var cvSearchModel = new CvSearchModel(
            newCv.Id, newCv.RowVersion, newCv.UserId,
            newCv.EducationRecords ?? [],
            newCv.WorkRecords ?? [],
            newCv.Skills ?? []
        );

        await transaction.CommitAsync(cancellationToken);

        backgroundJobService.Enqueue(
            () => cvSearchRepository.AddOrSetConstFieldsAsync(cvSearchModel, CancellationToken.None),
            BackgroundJobQueues.CvSearch
        );

        return Result.Success();
    }
}