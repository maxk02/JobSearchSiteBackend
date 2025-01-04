using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Domains.Cvs.Search;
using Core.Domains.Cvs.ValueEntities;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Core.Services.QueueService;
using Shared.Result;

namespace Core.Domains.Cvs.UseCases.AddCv;

public class AddCvHandler(
    ICurrentAccountService currentAccountService,
    ICvSearchRepository cvSearchRepository,
    IBackgroundJobQueueService jobQueueService,
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

        context.Cvs.Add(newCv);
        await context.SaveChangesAsync(cancellationToken);

        try
        {
            await cvSearchRepository.AddAsync(
                new CvSearchModel(newCv.Id, newCv.UserId, newCv.EducationRecords ?? new List<EducationRecord>(),
                    newCv.WorkRecords ?? new List<WorkRecord>(),
                    newCv.Skills ?? new List<string>(), new List<long>()),
                CancellationToken.None);
        }
        catch
        {
            await jobQueueService.EnqueueForIndefiniteRetriesAsync<ICvSearchRepository>(
                x => x.AddAsync(
                    new CvSearchModel(newCv.Id, newCv.UserId, newCv.EducationRecords ?? new List<EducationRecord>(),
                        newCv.WorkRecords ?? new List<WorkRecord>(),
                        newCv.Skills ?? new List<string>(), new List<long>()),
                    CancellationToken.None)
                );
        }

        return Result.Success();
    }
}