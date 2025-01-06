using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Domains.Cvs.Search;
using Core.Domains.Cvs.ValueEntities;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Core.Services.BackgroundJobService;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Cvs.UseCases.UpdateCv;

public class UpdateCvHandler(
    ICurrentAccountService currentAccountService,
    ICvSearchRepository cvSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<UpdateCvRequest, Result>
{
    public async Task<Result> Handle(UpdateCvRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var cv = await context.Cvs
            .Include(cv => cv.SalaryRecord)
            .Include(cv => cv.EmploymentTypeRecord)
            .Include(cv => cv.EducationRecords)
            .Include(cv => cv.WorkRecords)
            .FirstOrDefaultAsync(cv => cv.Id == request.CvId, cancellationToken);

        if (cv is null)
            return Result.NotFound();

        if (cv.UserId != currentUserId)
            return Result.Forbidden();

        var newCvCreationResult = Cv.Create(
            cv.UserId,
            request.SalaryRecord ?? cv.SalaryRecord,
            request.EmploymentTypeRecord ?? cv.EmploymentTypeRecord,
            request.EducationRecords ?? [..cv.EducationRecords ?? []],
            request.WorkRecords ?? [..cv.WorkRecords ?? []],
            request.Skills ?? [..cv.Skills ?? []]
        );

        if (newCvCreationResult.IsFailure)
            return newCvCreationResult;

        var newCv = newCvCreationResult.Value;

        if (request.CategoryIds is not null && request.CategoryIds.Count != 0)
        {
            if (request.CategoryIds.Count != request.CategoryIds.Distinct().Count())
                return Result.Invalid();

            var categoriesThatNotExist = request.CategoryIds.Except(Category.AllIds);

            if (categoriesThatNotExist.Any())
                return Result.Invalid();

            newCv.Categories = Category.AllValues.Where(c => request.CategoryIds.Contains(c.Id)).ToList();
        }

        var newCvSearchModel = new CvSearchModel(newCv.Id, newCv.UserId,
            newCv.EducationRecords ?? new List<EducationRecord>(),
            newCv.WorkRecords ?? new List<WorkRecord>(),
            newCv.Skills ?? new List<string>(),
            new List<long>(), new List<long>());

        context.Cvs.Update(newCv);
        await context.SaveChangesAsync(cancellationToken);
        
        backgroundJobService.Enqueue(
            () => cvSearchRepository.UpdateAsync(newCvSearchModel, CancellationToken.None),
            BackgroundJobQueues.CvSearch
        );
        
        return Result.Success();
    }
}