using System.Transactions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Domains.Cvs.Search;
using Core.Domains.Cvs.ValueEntities;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Ardalis.Result;

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
        context.Cvs.Add(newCv);
        
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        await context.SaveChangesAsync(cancellationToken);

        var cvSearchModel = new CvSearchModel(
            newCv.Id,
            newCv.EducationRecords ?? [],
            newCv.WorkRecords ?? [],
            newCv.Skills ?? []
        );
        
        backgroundJobService.Enqueue(
            () => cvSearchRepository
                .AddOrUpdateIfNewestAsync(cvSearchModel, newCv.RowVersion, CancellationToken.None),
            BackgroundJobQueues.CvSearch
        );

        transaction.Complete();

        return Result.Success();
    }
}